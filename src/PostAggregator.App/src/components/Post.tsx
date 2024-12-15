import React from "react";
import { Post as PostType } from "@/types/api/Post";
import { useNavigate } from "react-router-dom";

interface PostProps {
  post: PostType;
}

const Post: React.FC<PostProps> = ({ post }) => {
  const navigate = useNavigate();

  const handleClick = () => {
    if (post.link) {
      window.open(post.link, "_blank");
    } else {
      navigate(`/post/${post.id}`);
    }
  };

  return (
    <li
      onClick={handleClick}
      className={`p-6 rounded-lg shadow-lg transition duration-300 ease-in-out cursor-pointer group ${
        post.link
          ? "bg-white hover:bg-orange-700"
          : "bg-white hover:bg-orange-700"
      }`}
    >
      <div className="flex items-center space-x-4">
        {post.thumbnail && (
          <img
            src={post.thumbnail}
            alt={post.title}
            className="w-16 h-16 object-cover rounded-lg"
          />
        )}
        <div className="flex-1">
          <h2 className="text-xl font-semibold text-gray-900 transition duration-300 group-hover:text-white overflow-hidden overflow-ellipsis whitespace-normal line-clamp-2">
            {post.title}
          </h2>
        </div>
      </div>

      <div className="mt-2 text-gray-500 text-sm flex space-x-4">
        <p className="transition duration-300 group-hover:text-gray-300">
          By: {post.author}
        </p>
        <p className="transition duration-300 group-hover:text-gray-300">
          Posted on: {new Date(post.createdAtUtc).toLocaleDateString()}
        </p>
      </div>
    </li>
  );
};

export default Post;
