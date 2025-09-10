var toggle = document.getElementById('menu-toggle');
if (toggle) {
    toggle.addEventListener('click', function (e) {
        e.preventDefault();
        document.getElementById('sidebar').classList.toggle('d-none');
    });
}