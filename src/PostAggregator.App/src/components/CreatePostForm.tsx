import React, { useState } from "react";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";
import { createPost } from "@/services/postService";
import { CreatePost } from "@/types/api/CreatePost";
import Message from "@/components/Message";

const CreatePostForm: React.FC = () => {
  const [formData, setFormData] = useState<CreatePost>({
    title: "",
    author: "",
    text: "",
    thumbnail: undefined,
  });

  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  const handleChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const { name, value } = event.target;
    setFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleTextChange = (value: string) => {
    setFormData((prevData) => ({
      ...prevData,
      text: value,
    }));
  };

  const handleImageChange = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const file = event.target.files?.[0];
    if (file) {
      const base64 = await convertToBase64(file);
      setFormData((prevData) => ({
        ...prevData,
        thumbnail: base64,
      }));
    }
  };

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    setIsSubmitting(true);
    setError(null);
    setSuccess(null);

    if (!formData.text.trim()) {
      setError("Text is required.");
      setIsSubmitting(false);
      return;
    }

    const postData = { ...formData };

    try {
      await createPost(postData);
      setSuccess("Post created successfully!");

      setFormData({
        title: "",
        author: "",
        text: "",
        thumbnail: undefined,
      });
    } catch (error) {
      setError("Error creating post. Please try again.");
      console.error("Error creating post:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const convertToBase64 = (file: File): Promise<string> => {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = (error) => reject(error);
    });
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      {success && <Message type="success" message={success} />}
      {error && <Message type="error" message={error} />}

      <div>
        <label
          htmlFor="image"
          className="block text-lg font-medium text-gray-700"
        >
          Image (optional)
        </label>
        <div
          className="w-full p-3 border border-gray-300 rounded-md cursor-pointer"
          onClick={() => document.getElementById("image-input")?.click()}
          style={{
            minHeight: "200px",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            backgroundColor: "#f9f9f9",
            position: "relative",
          }}
        >
          {formData.thumbnail ? (
            <img
              src={formData.thumbnail}
              alt="Selected Thumbnail"
              className="object-cover w-full h-full rounded-md"
            />
          ) : (
            <span className="text-gray-500">No image selected</span>
          )}
          <input
            type="file"
            id="image-input"
            accept="image/*"
            onChange={handleImageChange}
            className="hidden"
          />
        </div>
      </div>

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
        <ReactQuill
          value={formData.text}
          onChange={handleTextChange}
          className="bg-white"
          style={{ height: "200px", resize: "vertical", overflow: "auto" }}
        />
      </div>

      <button
        type="submit"
        className="w-full bg-orange-600 text-white py-3 rounded-md button-animation text-lg"
        disabled={isSubmitting}
      >
        {isSubmitting ? "Submitting..." : "Create Post"}
      </button>
    </form>
  );
};

export default CreatePostForm;
