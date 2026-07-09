import { api } from '../api.js';

export function parseHashtags(text){
  return text.replace(/#(\w+)/g, (match, tag)=> {
    return `<a href="../../html/Posts/Search.html?tag=${tag}" 
          class="hashtag">#${tag}</a>`;
});
}
export function parseMentions(text) {
    return text.replace(/@(\w+)/g, (match, username) => {
        return `
            <a href="../../html/User/OtherUserProfile.html?username=${username}"
               class="mention" style="color:#7c3aed; text-decoration:none">
                @${username}
            </a>
        `;
    });
}
export function parseText(text) {
    let parsed = parseHashtags(text);
    parsed = parseMentions(parsed);
    return parsed;
}