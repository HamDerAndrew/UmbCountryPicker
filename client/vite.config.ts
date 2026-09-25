import { defineConfig } from "vite";

export default defineConfig({
  build: {
    lib: {
      entry: "src/country-picker.element.ts",
      formats: ["es"],
      fileName: () => "country-picker.js",
    },
    // wwwroot lives inside the .NET project and is served as a static web asset,
    // so it must never be wiped by the client build.
    outDir: "../wwwroot",
    emptyOutDir: false,
    sourcemap: true,
    rollupOptions: {
      // The backoffice supplies these at runtime via its import map.
      external: [/^@umbraco/],
    },
  },
  base: "/App_Plugins/UmbCountryPicker/",
});
