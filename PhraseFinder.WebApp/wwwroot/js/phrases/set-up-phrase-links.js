let selectedPhrase
const something = 'phrase-link'
let selectedPhraseLink

function setUpPhraseLinks() {
    document.querySelectorAll('[id^="phrase-link-"]').forEach(phraseLink => {
        phraseLink.addEventListener('click', e => handlePhraseLinkClick(e, phraseLink))
    })
}

function handlePhraseLinkClick(e, phraseLink) {
    const phraseId = phraseLink.id.replace('link-', '')

    selectedPhrase?.classList.remove('selected-phrase-card')
    selectedPhraseLink?.classList.remove('selected-phrase-link')

    if (selectedPhrase?.id === phraseId) {
        e.preventDefault()
        selectedPhrase = null
        selectedPhraseLink = null
        return
    }

    selectedPhrase = document.getElementById(phraseId)
    selectedPhrase?.classList.add('selected-phrase-card')

    selectedPhraseLink = phraseLink
    selectedPhraseLink.classList.add('selected-phrase-link')
}

setUpPhraseLinks()
