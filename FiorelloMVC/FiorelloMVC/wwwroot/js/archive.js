let categoryArchiveBtns = document.querySelectorAll(".add-archive");
let index = document.querySelectorAll(".restore");


categoryArchiveBtns.forEach(item =>

    item.addEventListener("click", function () {

        
        let id = this.getAttribute("data-id");
        (async () => {
            await fetch(`category/settoarchive?id=${id}`, {
                method: "POST",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
            });
            this.closest(".category-data").remove()
        })();
    })
)



index.forEach(item =>

    item.addEventListener("click", function () {

        let id = this.getAttribute("data-id");
        (async () => {
            await fetch(`/admin/archive/setfromarchive?id=${id}`, {
                method: "post",
                headers: {
                    'accept': 'application/json',
                    'content-type': 'application/json'
                },
            });
            this.closest(".category-data").remove()
        })();
    })
)

