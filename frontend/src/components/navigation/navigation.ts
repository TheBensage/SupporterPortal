import { Collapse } from "bootstrap";
import "./navigation.scss";

export default function Navigation(el: HTMLElement) {
  console.log("Navigation Initialised");

  const collapseEl = el.querySelector<HTMLElement>(".collapse");
  if (!collapseEl) return;
  new Collapse(collapseEl, { toggle: false });
}
