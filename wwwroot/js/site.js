// site.js

// Prosty przykład: dodajemy fade-in dla formularzy po wczytaniu strony
document.addEventListener("DOMContentLoaded", function () {
    const forms = document.querySelectorAll(".form-section");
    forms.forEach(form => {
        form.style.opacity = 0;
        form.style.transition = "opacity 0.5s ease-in-out";

        // Minimalne opóźnienie, żeby transition się zainicjowało
        setTimeout(() => {
            form.style.opacity = 1;
        }, 50);
    });

    // Możesz również dodać tutaj obsługę ewentualnych tooltipów, modali, itp.
    console.log("Strona załadowana – efekty JS aktywne.");
});
