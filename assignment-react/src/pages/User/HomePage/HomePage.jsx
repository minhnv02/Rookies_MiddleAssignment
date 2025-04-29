import React from "react";
import Home from "./components/Home";

const HomePage = ({ endpoint }) => {
  return (
    <div>
      <Home endpoint={endpoint} />
    </div>
  );
};

export default HomePage;
