export interface Post {
  id: string; // Guid is typically a string in API responses
  title: string;
  author: string;
  createdAtUtc: string; // This can be a string (ISO 8601 date format) or a Date object, depending on how it's returned
  link: string;
  thumbnail: string;
  text?: string;
}
