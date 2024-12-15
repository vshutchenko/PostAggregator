// src/services/postService.ts
import axios from "axios";
import { Post } from "../types/api/Post";
import { CreatePost } from "@/types/api/CreatePost";

// Function to generate mock posts based on the page number
const generateMockPosts = (
  pageNumber: number,
  postsPerPage: number
): Post[] => {
  const startIndex = (pageNumber - 1) * postsPerPage; // Calculate the starting index
  return Array.from({ length: postsPerPage }, (_, index) => ({
    id: `mock-id-${startIndex + index + 1}`, // IDs in ascending order based on page number
    title: `Mock Post Title ${startIndex + index + 1}`,
    author: `Author ${startIndex + index + 1}`,
    createdAtUtc: new Date().toISOString(),
    link: `https://example.com/mock-post/${startIndex + index + 1}`,
    thumbnail: "https://via.placeholder.com/150",
  }));
};

// Mock function to simulate fetching posts
export const fetchPosts = async (pageNumber: number): Promise<Post[]> => {
  // Simulate an API call with a delay
  await new Promise((resolve) => setTimeout(resolve, 1000));

  const postsPerPage = 9; // Define how many posts per page
  const mockPosts = generateMockPosts(pageNumber, postsPerPage); // Generate posts for the current page

  return mockPosts; // Return the generated posts

  return await axios
    .get<Post[]>(`/api/posts?pageNumber=${pageNumber}`)
    .then((response) => response.data);
};

// Function to fetch a single post by ID
export const fetchPostById = async (postId: string): Promise<Post> => {
  try {
    const response = await axios.get<Post>(`api/posts/${postId}`);
    return response.data; // Return the post data
  } catch (error) {
    console.error("Error fetching post:", error);
    throw error; // Rethrow the error for handling in the component
  }
};

// Function to create a new post
export const createPost = async (formData: CreatePost) => {
  try {
    const response = await axios.post("/api/posts", formData);
    return response.data; // Return the response data
  } catch (error) {
    console.error("Error creating post:", error);
    throw error; // Rethrow the error for handling in the component
  }
};
