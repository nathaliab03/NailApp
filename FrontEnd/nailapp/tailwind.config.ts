import type { Config } from "tailwindcss";

export default {
  content: [
    "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      colors: {
        background: "var(--background)",
        foreground: "var(--foreground)",
        primary: {
          DEFAULT: '#D9D9D9', // Cor padrão
          hover: '#C70039',  // Cor no hover
          disabled: '#FFC300', // Cor desabilitada
        },
      },
    },
  },
  plugins: [],
} satisfies Config;
