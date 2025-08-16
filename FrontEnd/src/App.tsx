import { Provider } from "react-redux";
import { BrowserRouter as Router } from "react-router-dom";
import { store } from "./redux/store";
import { ToastProvider } from "./components/common/ToastProvider";
import { AppRoutes } from "./routes/AppRoutes";

function App() {
  return (
    <Provider store={store}>
      <ToastProvider>
        <Router>
          <AppRoutes />
        </Router>
      </ToastProvider>
    </Provider>
  );
}

export default App;
