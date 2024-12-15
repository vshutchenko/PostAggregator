// src/components/PostList.tsx
import React, { useState, useEffect } from "react";
import { Post as PostType } from "@/types/api/Post"; // Import the Post model
import { fetchPosts } from "@/services/postService"; // Import the fetchPosts function
import Post from "@/components/Post"; // Import the Post component
import LoadingSpinner from "@/components/LoadingSpinner"; // Import the LoadingSpinner component

const PostList: React.FC = () => {
  const [posts, setPosts] = useState<PostType[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [pageNumber, setPageNumber] = useState<number>(1); // Track the current page

  const getPosts = async (page: number) => {
    setLoading(true); // Set loading to true before fetching
    try {
      const postsData = await fetchPosts(page); // Fetch posts with page number
      setPosts((prevPosts) => [...prevPosts, ...postsData]); // Append new posts to existing posts
    } catch (err) {
      console.error(err);
      setError("Failed to fetch posts");
    } finally {
      setLoading(false); // Set loading to false after fetching
    }
  };

  // Fetch posts when the component mounts and when pageNumber changes
  useEffect(() => {
    getPosts(pageNumber); // Fetch posts for the current page
  }, [pageNumber]); // Fetch posts when pageNumber changes

  useEffect(() => {
    const handleScroll = () => {
      // Check if the user has scrolled to the bottom of the page
      if (
        window.innerHeight + document.documentElement.scrollTop >=
        document.documentElement.offsetHeight - 250 // Allow a small buffer
      ) {
        // Only fetch more if not currently loading and there are existing posts
        if (!loading && posts.length > 0) {
          setPageNumber((prevPage) => prevPage + 1); // Increment page number
        }
      }
    };

    window.addEventListener("scroll", handleScroll);
    return () => {
      window.removeEventListener("scroll", handleScroll); // Cleanup listener on unmount
    };
  }, [loading, posts.length]); // Depend on loading and posts.length

  if (loading && posts.length === 0) {
    return <LoadingSpinner />; // Show loading spinner while loading initial posts
  }

  if (error) {
    return <p className="text-center text-xl text-red-600">{error}</p>;
  }

  return (
    <div className="space-y-6">
      <ul className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
        {posts.map((post, index) => (
          <Post key={index} post={post} /> // Use the Post component here
        ))}
      </ul>
      {loading && <LoadingSpinner />}{" "}
      {/* Show loading spinner when loading more posts */}
    </div>
  );
};

export default PostList;
