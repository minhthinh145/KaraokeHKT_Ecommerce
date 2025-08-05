import axios from "axios";

const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

let isRefreshing = false;
let failedQueue: any[] = [];

const processQueue = (error: any, token: string | null = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

// Request interceptor: tự động gắn accessToken
axiosInstance.interceptors.request.use((config) => {
  // Không gắn token cho signin/signup/refresh-token
  if (
    config.url &&
    (config.url.includes("/signin") ||
      config.url.includes("/signup") ||
      config.url.includes("/refresh-token"))
  ) {
    return config;
  }

  const accessToken = localStorage.getItem("accessToken");
  console.log("[DEBUG] AccessToken từ localStorage:", accessToken); // 🔥 DEBUG

  if (accessToken) {
    config.headers = config.headers ?? {};
    config.headers.Authorization = `Bearer ${accessToken}`;
    console.log("[DEBUG] Header Authorization:", config.headers.Authorization); // 🔥 DEBUG
  } else {
    console.log("[DEBUG] Không có accessToken trong localStorage"); // 🔥 DEBUG
  }

  return config;
});

// Response interceptor: tự động refresh token khi 401
axiosInstance.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    // Nếu lỗi 401 và chưa thử refresh
    if (
      error.response &&
      error.response.status === 401 &&
      !originalRequest._retry
    ) {
      if (isRefreshing) {
        // Nếu đang refresh, chờ refresh xong
        return new Promise(function (resolve, reject) {
          failedQueue.push({ resolve, reject });
        })
          .then((token) => {
            originalRequest.headers.Authorization = "Bearer " + token;
            return axiosInstance(originalRequest);
          })
          .catch((err) => Promise.reject(err));
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const refreshToken = localStorage.getItem("refreshToken");
        if (!refreshToken) throw new Error("No refresh token");

        // Gọi API refresh-token
        // Kiểu trả về: { AccessToken: string }
        const res = await axiosInstance.post<{ AccessToken: string }>(
          "/Auth/refresh-token",
          { refreshToken }
        );

        const { AccessToken } = res.data;

        // Lưu lại accessToken mới
        localStorage.setItem("accessToken", AccessToken);

        processQueue(null, AccessToken);

        // Gửi lại request cũ với token mới
        originalRequest.headers.Authorization = "Bearer " + AccessToken;
        return axiosInstance(originalRequest);
      } catch (err) {
        processQueue(err, null);

        return Promise.reject(err);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

export default axiosInstance;
