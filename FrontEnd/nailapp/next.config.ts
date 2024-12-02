import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  reactStrictMode: true,
  env: {
    API_BASE_URL: process.env.NEXT_PUBLIC_API_BASE_URL,
  },
};

console.log("Loaded API Base URL:", process.env.NEXT_PUBLIC_API_BASE_URL);

export default nextConfig;
