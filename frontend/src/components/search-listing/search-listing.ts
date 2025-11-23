interface SiteSearchResult {
  id: string;
  title: string;
  summary: string;
  url: string;
  contentType: string;
  imageUrl?: string;
}

interface SiteSearchResponse {
  results: SiteSearchResult[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export default function SearchListing(el: HTMLElement) {
  const container = el;
  const searchTerm = container.dataset.searchTerm || "";
  const contentTypes = container.dataset.contentTypes?.split(",") || [];
  const pageSize = parseInt(container.dataset.pageSize || "6");
  let currentPage = 1;

  const resultsContainer = document.createElement("div");
  resultsContainer.classList.add("row", "g-3");
  container.appendChild(resultsContainer);

  const paginationContainer = document.createElement("nav");
  paginationContainer.classList.add("mt-3");
  container.appendChild(paginationContainer);

  async function loadPage(page: number) {
    currentPage = page;
    const params = new URLSearchParams({
      searchTerm,
      page: currentPage.toString(),
      pageSize: pageSize.toString(),
      contentTypes: contentTypes.join(","),
    });

    const res = await fetch(`/api/search?${params}`);
    const data: SiteSearchResponse = await res.json();

    renderResults(data);
    renderPagination(data);
  }

  function renderResults(data: SiteSearchResponse) {
    resultsContainer.innerHTML = "";

    if (!data.results.length) {
      resultsContainer.innerHTML = `<div class="col-12"><p>No results found.</p></div>`;
      return;
    }

    const fragment = document.createDocumentFragment();

    data.results.forEach((item) => {
      const col = document.createElement("div");
      col.className = "col-md-4";

      const card = document.createElement("div");
      card.className = "card h-100";

      if (item.imageUrl) {
        const img = document.createElement("img");
        img.className = "card-img-top";
        img.src = item.imageUrl;
        img.alt = item.title;
        card.appendChild(img);
      }

      const body = document.createElement("div");
      body.className = "card-body";

      const title = document.createElement("h5");
      title.className = "card-title";
      title.textContent = item.title;

      const summary = document.createElement("p");
      summary.className = "card-text";
      summary.textContent = item.summary;

      const link = document.createElement("a");
      link.className = "btn btn-primary";
      link.href = item.url;
      link.textContent = "Read more";

      body.append(title, summary, link);
      card.appendChild(body);
      col.appendChild(card);
      fragment.appendChild(col);
    });

    resultsContainer.appendChild(fragment);
  }

  function renderPagination(data: SiteSearchResponse) {
    const totalPages = Math.ceil(data.totalCount / data.pageSize);
    paginationContainer.innerHTML = ""; // reset

    if (totalPages <= 1) return;

    const ul = document.createElement("ul");
    ul.className = "pagination";

    for (let i = 1; i <= totalPages; i++) {
      const li = document.createElement("li");
      li.className = `page-item ${i === data.page ? "active" : ""}`;

      const a = document.createElement("a");
      a.className = "page-link";
      a.href = "#";
      a.textContent = i.toString();
      a.addEventListener("click", (e) => {
        e.preventDefault();
        if (i !== currentPage) loadPage(i);
      });

      li.appendChild(a);
      ul.appendChild(li);
    }

    paginationContainer.appendChild(ul);
  }
  loadPage(1);
}
