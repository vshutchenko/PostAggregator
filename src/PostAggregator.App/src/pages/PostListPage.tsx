// src/pages/PostListPage.tsx
import React from "react";
import PostList from "@/components/PostList";

const PostListPage: React.FC = () => {
  return (
    <div>
      <h1 className="text-4xl font-bold text-center text-blue-600 mb-8">
        Posts
      </h1>
      <PostList />
    </div>
  );
};

export default PostListPage;
