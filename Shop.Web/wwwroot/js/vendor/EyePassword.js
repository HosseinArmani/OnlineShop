const passField = document.getElementById('pas'),
    showBtn = document.getElementById('eye');

showBtn.addEventListener('click', () => {
    if (passField.type === 'password') {
        passField.type = 'text';
        showBtn.classList.add('hide');
    } else {
        passField.type = 'password';
        showBtn.classList.remove('hide');
    }
});