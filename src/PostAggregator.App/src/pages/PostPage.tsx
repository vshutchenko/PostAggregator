import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { fetchPostById } from "@/services/postService";
import { Post } from "@/types/api/Post";
import LoadingSpinner from "@/components/LoadingSpinner";
import Message from "@/components/Message";
import ReactQuill from "react-quill";

const PostPage: React.FC = () => {
  const { postId } = useParams<{ postId: string }>();
  const [post, setPost] = useState<Post | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const getPost = async () => {
      try {
        const fetchedPost = await fetchPostById(postId!);
        setPost(fetchedPost);
      } catch (err) {
        console.error(err);
        setError("Failed to fetch post");
      } finally {
        setLoading(false);
      }
    };

    getPost();
  }, [postId]);

  if (loading) {
    return <LoadingSpinner />;
  }

  if (error) {
    return <Message type="error" message={error} />;
  }

  if (!post) {
    return <p>Post not found.</p>;
  }

  return (
    <div className="max-w-2xl mx-auto p-6">

      <div className="bg-white p-6 rounded-lg shadow-lg">
        <h2 className="text-4xl font-bold text-center text-orange-600 mb-8">
          {post.title}
        </h2>
        <h2 className="text-2xl font-semibold text-gray-900">
          By {post.author}
        </h2>

        {post.thumbnail && (
          <div className="my-6">
            <img
              src={post.thumbnail}
              alt="Post Thumbnail"
              className="w-full h-64 object-cover rounded-lg shadow-md"
            />
          </div>
        )}
        <ReactQuill
          value={post.text}
          readOnly={true}
          theme={"bubble"}
        />
      </div>
    </div>
  );
};

export default PostPage;
