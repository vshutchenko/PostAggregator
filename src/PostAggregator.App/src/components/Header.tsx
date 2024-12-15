import React from "react";
import { NavLink } from "react-router-dom";

const Header: React.FC = () => {
  return (
    <header className="bg-orange-700 text-white py-4 shadow-lg">
      <div className="max-w-6xl mx-auto flex justify-between items-center px-6 sm:px-8">
        <h1 className="text-3xl sm:text-4xl font-extrabold tracking-tight hover:text-gray-200 transition-all">
          <NavLink to="/" end>
            Post aggregator
          </NavLink>
        </h1>
        <nav>
          <ul className="flex space-x-8 text-lg sm:text-xl">
            <li>
              <NavLink
                to="/"
                className={({ isActive }) =>
                  isActive
                    ? "text-yellow-400"
                    : "text-white hover:text-gray-200 transition-colors duration-300"
                }
              >
                Recent Posts
              </NavLink>
            </li>
            <li>
              <NavLink
                to="/create"
                className={({ isActive }) =>
                  isActive
                    ? "text-yellow-400"
                    : "text-white hover:text-gray-200 transition-colors duration-300"
                }
              >
                Create Post
              </NavLink>
            </li>
          </ul>
        </nav>
      </div>
    </header>
  );
};

export default Header;
