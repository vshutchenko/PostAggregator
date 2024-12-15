import React from "react";
import { Link } from "react-router-dom";

const NotFoundPage: React.FC = () => {
  return (
    <div className="flex flex-col items-center justify-center h-screen">
      <h1 className="text-6xl font-bold text-gray-800">404</h1>
      <p className="text-xl text-gray-600 mb-4">Page Not Found</p>
      <Link
        to="/"
        className="bg-orange-600 text-white py-2 px-4 rounded-md hover:bg-orange-700 transition duration-300 button-animation"
      >
        Go back to Home
      </Link>
    </div>
  );
};

export default NotFoundPage;
