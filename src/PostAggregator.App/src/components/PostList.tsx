import React, { useState, useEffect } from "react";
import { Post as PostType } from "@/types/api/Post";
import { fetchPosts } from "@/services/postService";
import Post from "@/components/Post";
import LoadingSpinner from "@/components/LoadingSpinner";
import Message from "@/components/Message";
import { toast } from "react-hot-toast";

const PostList: React.FC = () => {
  const [posts, setPosts] = useState<PostType[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [pageNumber, setPageNumber] = useState<number>(1);

  const getPosts = async (page: number) => {
    setLoading(true);
    try {
      const postsData = await fetchPosts(page);

      if (postsData.length === 0) {
        toast.success("All posts have been loaded!");
      } else {
        setPosts((prevPosts) => [...prevPosts, ...postsData]);
      }
    } catch (err) {
      console.error(err);
      setError("Failed to fetch posts");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    getPosts(pageNumber);
  }, [pageNumber]);

  const loadMorePosts = () => {
    if (!loading) {
      setPageNumber((prevPage) => prevPage + 1);
    }
  };

  if (loading && posts.length === 0) {
    return <LoadingSpinner />;
  }

  if (error) {
    return <Message type="error" message={error} />;
  }

  return (
    <div className="space-y-6">
      <ul className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
        {posts.map((post, index) => (
          <Post key={index} post={post} />
        ))}
      </ul>
      {loading && <LoadingSpinner />}
      {!loading && posts.length > 0 && (
        <div className="flex justify-center mt-4">
          <button
            onClick={loadMorePosts}
            className="bg-orange-600 text-white py-2 px-4 rounded transition-transform transform button-animation"
          >
            Load More
          </button>
        </div>
      )}
    </div>
  );
};

export default PostList;
