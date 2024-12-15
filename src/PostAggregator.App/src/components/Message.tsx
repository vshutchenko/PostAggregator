import React from "react";

interface MessageProps {
  type: "success" | "error";
  message: string;
}

const Message: React.FC<MessageProps> = ({ type, message }) => {
  const bgColor = type === "success" ? "bg-green-200" : "bg-red-200";
  const textColor = type === "success" ? "text-green-800" : "text-red-800";

  return (
    <div
      className={`${bgColor} ${textColor} p-4 mb-4 rounded-md text-center max-w-md mx-auto`}
    >
      {message}
    </div>
  );
};

export default Message;
