// src/pages/PostPage.tsx
import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { fetchPostById } from "@/services/postService"; // Import the fetchPostById function
import { Post } from "@/types/api/Post"; // Import the Post type

const PostPage: React.FC = () => {
  const { postId } = useParams<{ postId: string }>(); // Get the postId from the URL
  const [post, setPost] = useState<Post | null>(null); // State to hold the post data
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const getPost = async () => {
      try {
        const fetchedPost = await fetchPostById(postId!); // Fetch the post by ID
        setPost(fetchedPost); // Set the post data
      } catch (err) {
        console.error(err);
        setError("Failed to fetch post");
      } finally {
        setLoading(false); // Set loading to false after fetching
      }
    };

    getPost(); // Call the function to fetch the post
  }, [postId]);

  if (loading) {
    return <p>Loading...</p>; // Show loading state
  }

  if (error) {
    return <p className="text-red-600">{error}</p>; // Show error message
  }

  if (!post) {
    return <p>Post not found.</p>; // Handle case where post is not found
  }

  return (
    <div className="max-w-2xl mx-auto p-6">
      <h1 className="text-4xl font-bold text-center text-blue-600 mb-8">
        {post.title}
      </h1>
      <div className="bg-white p-6 rounded-lg shadow-lg">
        <h2 className="text-2xl font-semibold text-gray-900">
          By {post.author}
        </h2>
        <p className="text-gray-700 mt-4">{post.text}</p>
      </div>
    </div>
  );
};

export default PostPage;
