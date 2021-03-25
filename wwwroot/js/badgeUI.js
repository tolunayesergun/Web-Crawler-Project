const badgeContainer = document.querySelector(".container-badge");
const badgeAddList = document.querySelectorAll(".badge-add");


badgeContainer.addEventListener("click", function (e) {
    if (e.target.classList.contains("close")) {
        e.target.parentNode.remove();
    } else if (e.target.parentNode.classList.contains("close")) {
        e.target.parentNode.parentNode.remove();
    }
});

badgeAddList.forEach(badgeAdd => {
    badgeAdd.addEventListener("keypress", function (e) {
        if (e.keyCode === 13) {
            e.preventDefault();
            const badge = document.createElement("span");
            badge.innerHTML = this.innerHTML;
            badge.classList.add("badge", "badge-default", "badge-closable");

            if (this.dataset.badgeClass) {
                badge.classList.add(this.dataset.badgeClass);
            }

            const close = document.createElement("span");
            close.classList.add("close");

            const icon = document.createElement("i");
            icon.classList.add("fa", "fa-times");

            close.appendChild(icon);
            badge.appendChild(close);

            this.parentNode.insertBefore(badge, this);
            this.innerHTML = "";
        }
    });
});