interface NewsletterFormResponse {
  success: boolean;
  message?: string;
  errors?: Record<string, string[] | null>;
}

export default function NewsletterForm(el: HTMLElement) {
  const form = el.querySelector("form");
  const feedback = el.querySelector("#newsletterFeedback");
  const errorText = el.dataset.errorText;

  if (!form || !feedback) return;

  function setFieldError(input: HTMLInputElement, message: string) {
    input.classList.add("is-invalid");
    input.setCustomValidity(message);
    const span = input
      .closest(".js-form-field")
      ?.querySelector(".invalid-feedback");
    if (span) span.textContent = message;
  }

  form
    .querySelectorAll<HTMLInputElement>("input, textarea")
    .forEach((input) => {
      input.addEventListener("input", () => {
        input.classList.remove("is-invalid");
        input.setCustomValidity("");
        const span = input
          .closest(".js-form-field")
          ?.querySelector(".invalid-feedback");
        if (span) span.textContent = "";
      });
    });

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
      const res = await fetch("/api/newsletter/subscribe", {
        method: "POST",
        body: formData,
      });

      const data = (await res.json()) as NewsletterFormResponse;

      if (data.success) {
        feedback.innerHTML = `<div class="alert alert-success">${data.message}</div>`;
        form.reset();
        form.classList.remove("was-validated");
        form
          .querySelectorAll<HTMLInputElement>("input, textarea")
          .forEach((i) => i.setCustomValidity(""));
      } else if (data.errors) {
        form.classList.add("was-validated");
        for (const key of Object.keys(data.errors)) {
          const input = form.querySelector(
            `[name="${key}"]`
          ) as HTMLInputElement | null;
          if (input && data.errors[key]) {
            setFieldError(input, data.errors[key]!.join(", "));
          }
        }
      }
    } catch (err) {
      console.error(err);
      feedback.innerHTML = `<div class="alert alert-danger">${errorText}</div>`;
    }
  });
}
