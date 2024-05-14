function padBodyTop() {
    const header = document.querySelector('header')
    const body = document.querySelector('body')
    body.style.paddingTop = `${header.offsetHeight}px`
}

window.addEventListener('load', padBodyTop)
window.addEventListener('resize', padBodyTop)
