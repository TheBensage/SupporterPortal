import { Collapse } from "bootstrap";

export default function Navigation(el: HTMLElement) {
  const collapseEl = el.querySelector<HTMLElement>(".collapse");
  if (!collapseEl) return;
  new Collapse(collapseEl, { toggle: false });
}
