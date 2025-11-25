import "./search-listing.scss";

interface SiteSearchResult {
  id: string;
  title: string;
  summary: string;
  linkText: string;
  url: string;
  contentType: string;
  imageUrl?: string;
}

interface SiteSearchResponse {
  results: SiteSearchResult[];
  totalCount: number;
  page: number;
  pageSize: number;
  error: string | null;
  message: string | null;
}

export default function SearchListing(el: HTMLElement) {
  const container = el;

  const searchTerm = container.dataset.searchTerm || "";
  const contentTypes = container.dataset.contentTypes?.split(",") || [];
  const pageSize = parseInt(container.dataset.pageSize || "6");
  const orderBy = container.dataset.orderBy || "";
  const maxSize = parseInt(container.dataset.maxSize || "-1");
  const noResultsText = container.dataset.noResultsText;

  let currentPage = 1;

  const resultsWrapper = document.createElement("div");
  resultsWrapper.classList.add("position-relative", "d-flex", "flex-column");
  container.appendChild(resultsWrapper);

  const resultsContainer = document.createElement("div");
  resultsContainer.classList.add("row", "g-3", "search-results-container");
  resultsWrapper.appendChild(resultsContainer);

  const paginationContainer = document.createElement("nav");
  paginationContainer.classList.add("mt-3");
  container.appendChild(paginationContainer);

  const loadingIndicator = document.createElement("div");
  loadingIndicator.className =
    "search-loading position-absolute top-50 start-50 translate-middle mb-5";
  loadingIndicator.innerHTML = `
    <div class="spinner-border text-primary" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  `;
  resultsWrapper.appendChild(loadingIndicator);

  let paginationList: HTMLUListElement | null = null;
  let pageItems: HTMLLIElement[] = [];

  async function loadPage(page: number) {
    currentPage = page;
    loadingIndicator.classList.remove("d-none");

    const params = new URLSearchParams({
      searchTerm,
      page: currentPage.toString(),
      pageSize: pageSize.toString(),
      contentTypes: contentTypes.join(","),
    });
    if (orderBy) params.set("orderBy", orderBy);
    if (maxSize > 0) params.set("maxSize", maxSize.toString());

    try {
      const res = await fetch(`/api/search?${params}`);
      const data: SiteSearchResponse = await res.json();

      resultsContainer.innerHTML = "";

      if (!res.ok || data.error) {
        const errorMsg = data.error || `HTTP error! status: ${res.status}`;
        resultsContainer.innerHTML = `<div class="col-12"><p class="text-danger">${errorMsg}</p></div>`;
        paginationContainer.innerHTML = "";
        paginationList = null;
        pageItems = [];
        return;
      }

      if (!data.results.length) {
        const msg =
          data.message ||
          noResultsText ||
          `No results found for "${searchTerm}".`;
        resultsContainer.innerHTML = `<div class="col-12"><p>${msg}</p></div>`;
      } else {
        const fragment = document.createDocumentFragment();
        data.results.forEach((item, idx) => {
          const col = document.createElement("div");
          col.className = "col-md-4 search-result-item";
          col.style.animationDelay = `${idx * 100}ms`;

          const card = document.createElement("div");
          card.className = "card h-100 shadow-sm border-0";

          if (item.imageUrl) {
            const img = document.createElement("img");
            img.className = "card-img-top";
            img.src = item.imageUrl;
            img.alt = item.title;
            card.appendChild(img);
          }

          const body = document.createElement("div");
          body.className = "card-body";

          const title = document.createElement("h3");
          title.className = "card-title";
          title.textContent = item.title;

          const summary = document.createElement("p");
          summary.className = "card-text";
          summary.textContent = item.summary;

          const link = document.createElement("a");
          link.className = "btn btn-primary";
          link.href = item.url;
          link.textContent = item.linkText;

          body.append(title, summary, link);
          card.appendChild(body);
          col.appendChild(card);
          fragment.appendChild(col);
        });

        resultsContainer.appendChild(fragment);
      }

      renderPagination(data);
    } catch (err: any) {
      console.error("Error loading search results:", err);
      const msg = err?.message || "Failed to load results.";
      resultsContainer.innerHTML = `<div class="col-12"><p class="text-danger">${msg}</p></div>`;
      paginationContainer.innerHTML = "";
      paginationList = null;
      pageItems = [];
    } finally {
      loadingIndicator.classList.add("d-none");
    }
  }

  function renderPagination(data: SiteSearchResponse) {
    const totalPages = Math.ceil(data.totalCount / data.pageSize);
    if (totalPages <= 1) {
      paginationContainer.innerHTML = "";
      paginationList = null;
      pageItems = [];
      return;
    }

    if (!paginationList || pageItems.length !== totalPages) {
      paginationContainer.innerHTML = "";
      paginationList = document.createElement("ul");
      paginationList.classList.add("pagination", "justify-content-center");
      pageItems = [];

      for (let i = 1; i <= totalPages; i++) {
        const li = document.createElement("li");
        li.className = `page-item ${i === currentPage ? "active" : ""}`;

        const a = document.createElement("a");
        a.className = "page-link";
        a.href = "#";
        a.textContent = i.toString();
        a.addEventListener("click", (e) => {
          e.preventDefault();
          if (i !== currentPage) loadPage(i);
        });

        li.appendChild(a);
        paginationList.appendChild(li);
        pageItems.push(li);
      }

      paginationContainer.appendChild(paginationList);
    } else {
      pageItems.forEach((li, idx) => {
        li.classList.toggle("active", idx + 1 === currentPage);
      });
    }
  }

  loadPage(1);
}
