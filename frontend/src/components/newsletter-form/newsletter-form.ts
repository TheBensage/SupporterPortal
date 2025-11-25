interface NewsletterFormResponse {
  success: boolean;
  message?: string;
  errors?: Record<string, string[] | null>;
}

export default function NewsletterForm(el: HTMLElement) {
  const form = el as HTMLFormElement;
  const feedback = el.querySelector("#newsletterFeedback");
  const errorText = el.dataset.errorText;

  if (!form || !feedback) return;

  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    form
      .querySelectorAll(".is-invalid")
      .forEach((i) => i.classList.remove("is-invalid"));
    form
      .querySelectorAll(".invalid-feedback")
      .forEach((span) => (span.textContent = ""));
    feedback.innerHTML = "";

    if (!form.checkValidity()) {
      form.classList.add("was-validated");
      return;
    }

    const formData = new FormData(form);

    try {
      const res = await fetch("/api/subscribe", {
        method: "POST",
        body: formData,
      });

      const data = (await res.json()) as NewsletterFormResponse;

      if (data.success) {
        feedback.innerHTML = `<div class="alert alert-success">${data.message}</div>`;
        form.reset();
        form.classList.remove("was-validated");
      } else if (data.errors) {
        for (const key of Object.keys(data.errors)) {
          const input = form.querySelector(
            `[name="${key}"]`
          ) as HTMLInputElement | null;
          const span = input?.nextElementSibling as HTMLElement | null;
          if (input && span && data.errors[key]) {
            input.classList.add("is-invalid");
            span.textContent = data.errors[key]!.join(", ");
          }
        }
      }
    } catch (err) {
      console.error(err);
      feedback.innerHTML = `<div class="alert alert-danger">${errorText}</div>`;
    }
  });
}
