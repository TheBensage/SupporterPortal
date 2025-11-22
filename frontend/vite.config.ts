import { defineConfig } from "vite";
import path from "path";
import fg from "fast-glob";

function getAllEntries() {
  const files = fg.sync("src/**/*.ts", { absolute: true });
  const entries: Record<string, string> = {};

  files.forEach((file) => {
    const name = path.basename(file, ".ts");
    entries[name] = file;
  });

  return entries;
}

export default defineConfig({
  base: "/dist/",
  root: path.resolve(__dirname, "src"),
  build: {
    outDir: path.resolve(__dirname, "../SupporterPortal.Web/wwwroot/dist"),
    emptyOutDir: true,
    cssCodeSplit: true,
    rollupOptions: {
      input: getAllEntries(),
      output: {
        entryFileNames: "js/[name].js",
        chunkFileNames: "js/[name]-[hash].js",
        assetFileNames: (assetInfo) => {
          if (assetInfo.name?.endsWith(".css")) {
            const baseName = path.basename(assetInfo.name, ".css");
            return `css/${baseName}.css`;
          }
          return "[name][extname]";
        },
      },
    },
  },
});
