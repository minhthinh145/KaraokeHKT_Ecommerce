export function formatDateForDisplay(dateInput?: string | Date | null): string {
  if (!dateInput) return "-";
  let date: Date;
  if (typeof dateInput === "string") {
    date = new Date(dateInput);
  } else {
    date = dateInput;
  }
  if (isNaN(date.getTime())) return "-";
  // Format: dd/MM/yyyy
  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const year = date.getFullYear();
  return `${day}/${month}/${year}`;
}
export function formatDateForInput(dateInput?: string | Date | null): string {
  if (!dateInput) return "";
  let date: Date;
  if (typeof dateInput === "string") {
    date = new Date(dateInput);
  } else {
    date = dateInput;
  }
  if (isNaN(date.getTime())) return "";
  // Format: yyyy-MM-dd
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
}
export function formatDateTimeForDisplay(
  dateInput?: string | Date | null
): string {
  if (!dateInput) return "-";
  let date: Date;
  if (typeof dateInput === "string") {
    date = new Date(dateInput);
  } else {
    date = dateInput;
  }
  if (isNaN(date.getTime())) return "-";
  // Format: dd/MM/yyyy HH:mm
  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const year = date.getFullYear();
  const hours = String(date.getHours()).padStart(2, "0");
  const minutes = String(date.getMinutes()).padStart(2, "0");
  return `${day}/${month}/${year} ${hours}:${minutes}`;
}
export function formatDateTimeForInput(
  dateInput?: string | Date | null
): string {
  if (!dateInput) return "";
  let date: Date;
  if (typeof dateInput === "string") {
    date = new Date(dateInput);
  } else {
    date = dateInput;
  }
  if (isNaN(date.getTime())) return "";
  // Format: yyyy-MM-ddTHH:mm
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");
  const hours = String(date.getHours()).padStart(2, "0");
  const minutes = String(date.getMinutes()).padStart(2, "0");
  return `${year}-${month}-${day}T${hours}:${minutes}`;
}
