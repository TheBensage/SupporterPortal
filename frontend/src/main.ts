import "./main.scss";

const modules = import.meta.glob("./components/*/*.ts", { eager: false });

const componentMap: Record<
  string,
  () => Promise<{ default: (el: HTMLElement) => void }>
> = {};

for (const [path, loader] of Object.entries(modules)) {
  const match = path.match(/\.\/components\/([^/]+)\/\1\.ts$/);
  if (match) {
    const name = match[1];
    componentMap[name] = loader as () => Promise<{
      default: (el: HTMLElement) => void;
    }>;
  }
}

document.addEventListener("DOMContentLoaded", () => {
  const elements = document.querySelectorAll<HTMLElement>("[data-component]");

  const observer = new IntersectionObserver(
    (entries, obs) => {
      entries.forEach(async (entry, index) => {
        if (!entry.isIntersecting) return;

        const el = entry.target as HTMLElement;
        const name = el.dataset.component;
        if (!name || !componentMap[name]) return;
        const module = await componentMap[name]();
        module.default(el);
        setTimeout(() => {
          el.classList.add("is-initialized");
        }, index * 150);
        obs.unobserve(el);
      });
    },
    { threshold: 0.1 }
  );

  elements.forEach((el) => observer.observe(el));
});
