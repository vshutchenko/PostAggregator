import React from "react";
import CreatePostForm from "@/components/CreatePostForm";

const CreatePostPage: React.FC = () => {
  return (
    <div className="max-w-2xl mx-auto p-6">
      <h1 className="text-4xl font-bold text-center text-orange-600 mb-8">
        Create Post
      </h1>
      <CreatePostForm />
    </div>
  );
};

export default CreatePostPage;
