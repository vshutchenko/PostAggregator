import React, { useState } from "react";
import { CreatePost } from "@/types/api/CreatePost"; // Import CreatePostDto type
import { createPost } from "@/services/postService";

const CreatePostPage: React.FC = () => {
  // Step 2: Create state for form data
  const [formData, setFormData] = useState<CreatePost>({
    title: "",
    author: "",
    text: "",
  });

  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  // Step 3: Handle form field changes
  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  // Step 4: Handle form submission (mock API request)
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    // Basic validation
    if (!formData.title || !formData.author || !formData.text) {
      setError("All fields are required.");
      return;
    }

    setError(null);
    setIsSubmitting(true);

    // Mock API submission (replace with real API call later)
    try {
      // You can mock API call here, like an Axios POST request:
      // await axios.post('/api/posts', formData);
      await createPost(formData);

      // Simulate a successful post creation
      setTimeout(() => {
        setIsSubmitting(false);
        setSuccessMessage("Post created successfully!");
        setFormData({ title: "", author: "", text: "" }); // Reset form
      }, 1500);
    } catch (err) {
      console.error(err);
      setIsSubmitting(false);
      setError("An error occurred while creating the post.");
    }
  };

  return (
    <div className="max-w-md mx-auto p-6">
      <h1 className="text-4xl font-bold text-center text-blue-600 mb-8">
        Create Post
      </h1>

      {successMessage && (
        <div className="bg-green-200 text-green-800 p-4 mb-4 rounded-md text-center">
          {successMessage}
        </div>
      )}

      {error && (
        <div className="bg-red-200 text-red-800 p-4 mb-4 rounded-md text-center">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label
            htmlFor="title"
            className="block text-lg font-medium text-gray-700"
          >
            Title
          </label>
          <input
            type="text"
            id="title"
            name="title"
            value={formData.title}
            onChange={handleChange}
            className="w-full p-3 border border-gray-300 rounded-md"
            required
          />
        </div>

        <div>
          <label
            htmlFor="author"
            className="block text-lg font-medium text-gray-700"
          >
            Author
          </label>
          <input
            type="text"
            id="author"
            name="author"
            value={formData.author}
            onChange={handleChange}
            className="w-full p-3 border border-gray-300 rounded-md"
            required
          />
        </div>

        <div>
          <label
            htmlFor="text"
            className="block text-lg font-medium text-gray-700"
          >
            Text
          </label>
          <textarea
            id="text"
            name="text"
            value={formData.text}
            onChange={handleChange}
            rows={5}
            className="w-full p-3 border border-gray-300 rounded-md"
            required
          />
        </div>

        <button
          type="submit"
          className="w-full bg-blue-600 text-white py-3 rounded-md"
          disabled={isSubmitting}
        >
          {isSubmitting ? "Submitting..." : "Create Post"}
        </button>
      </form>
    </div>
  );
};

export default CreatePostPage;
