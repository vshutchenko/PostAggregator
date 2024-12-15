// src/components/Post.tsx
import React from "react";
import { Post as PostType } from "@/types/api/Post"; // Import the Post type
import { useNavigate } from "react-router-dom"; // Import useNavigate and Link

interface PostProps {
  post: PostType; // Define props type
}

const Post: React.FC<PostProps> = ({ post }) => {
  const navigate = useNavigate(); // Initialize useNavigate

  const handleClick = () => {
    navigate(`/post/${post.id}`); // Navigate to the post detail page
  };

  return (
    <li
      onClick={handleClick} // Add click handler
      className="bg-white p-6 rounded-lg shadow-lg hover:bg-blue-700 transition duration-300 ease-in-out cursor-pointer group" // Add group class
    >
      <h2 className="text-2xl font-semibold text-gray-900 transition duration-300 group-hover:text-white">
        {post.title}
      </h2>
      <p className="mt-2 text-gray-500 transition duration-300 group-hover:text-gray-300">
        By: {post.author}
      </p>
      <p className="mt-2 text-gray-500 text-sm transition duration-300 group-hover:text-gray-300">
        Created on: {new Date(post.createdAtUtc).toLocaleDateString()}
      </p>
      <p className="mt-4 text-blue-500 transition duration-300 group-hover:text-yellow-400">
        Read more
      </p>
      {post.thumbnail && (
        <img
          src={post.thumbnail}
          alt={post.title}
          className="mt-4 w-full h-48 object-cover rounded-lg"
        />
      )}
    </li>
  );
};

export default Post;
