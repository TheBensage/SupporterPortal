export default function NewsletterForm(el: HTMLElement) {
  const form = el as HTMLFormElement;
  const feedback = el.querySelector("#newsletterFeedback");

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

      const data = await res.json();

      if (data.success) {
        feedback.innerHTML = `<div class="alert alert-success">${data.message}</div>`;
        form.reset();
        form.classList.remove("was-validated");
      } else {
        for (const key in data.errors) {
          const input = form.querySelector(`[name="${key}"]`);
          const span = input?.nextElementSibling;
          if (input && span) {
            input.classList.add("is-invalid");
            span.textContent = data.errors[key].join(", ");
          }
        }
      }
    } catch (err) {
      console.error(err);
      feedback.innerHTML = `<div class="alert alert-danger">Error submitting form.</div>`;
    }
  });
}
