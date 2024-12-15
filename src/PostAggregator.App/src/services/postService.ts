import axios from "axios";
import { Post } from "../types/api/Post";
import { CreatePost } from "@/types/api/CreatePost";

const apiUrl = import.meta.env.VITE_API_URL;

export const fetchPosts = async (pageNumber: number): Promise<Post[]> => {
  return await axios
    .get<Post[]>(`${apiUrl}/posts?page=${pageNumber}`)
    .then((response) => response.data);
};

export const fetchPostById = async (postId: string): Promise<Post> => {
  try {
    const response = await axios.get<Post>(`${apiUrl}/posts/${postId}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching post:", error);
    throw error;
  }
};

export const createPost = async (formData: CreatePost) => {
  try {
    const response = await axios.post(`${apiUrl}/posts`, formData);
    return response.data;
  } catch (error) {
    console.error("Error creating post:", error);
    throw error;
  }
};
