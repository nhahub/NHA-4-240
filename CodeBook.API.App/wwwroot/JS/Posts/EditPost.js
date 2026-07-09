import { api } from '../api.js';

const urlParams = new URLSearchParams(window.location.search);
const postId = urlParams.get('id');
const referrer = document.referrer;
   const errorMsg = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');

document.addEventListener('DOMContentLoaded', () => {
    if (!postId) {
        window.location.href = 'Feed.html';
        return;
    }
    loadPost();
});

async function loadPost() {
    try {
        const post = await api.get(`Post/${postId}`);

        if (!post) {
            window.location.href = 'Feed.html';
            return;
        }

        document.getElementById('title').value = post.title || '';
        document.getElementById('body').value = post.body || '';
        document.getElementById('codeSnippet').value = post.codeSnippet || '';
        document.getElementById('isPublic').checked = post.isPublic;

        const languageSelect = document.getElementById('language');
        for (let option of languageSelect.options) {
            if (option.value === post.language) {
                option.selected = true;
                break;
            }
        }

    } catch (error) {
        console.error('Failed to load post: ', error);
    }
}

async function updatePost() {
    const title = document.getElementById('title').value.trim();
    const body = document.getElementById('body').value.trim();
    const codeSnippet = document.getElementById('codeSnippet').value.trim();
    const language = document.getElementById('language').value;
    const isPublic = document.getElementById('isPublic').checked;

    const errorMsg = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');

    errorMsg.style.display = 'none';
    successMsg.style.display = 'none';

    if (!title) {
        errorMsg.textContent = 'Title is required!';
        errorMsg.style.display = 'block';
        return;
    }

    if (!body) {
        errorMsg.textContent = 'Body is required!';
        errorMsg.style.display = 'block';
        return;
    }

    try {
        const result = await api.put(`Post/${postId}/update`, {
            title: title,
            body: body,
            codeSnippet: codeSnippet || null,
            language: language || null,
            isPublic: isPublic,
            communityId: null
        });

        if (result.message === 'Post updated successfully') {
            successMsg.textContent = 'Post updated successfully!';
            successMsg.style.display = 'block';

            setTimeout(() => {
            if (referrer) {
                window.location.href = referrer;
            } else {
                window.location.href = `PostDetail.html?id=${postId}`;
            }
         }, 1500);
        } else {
            errorMsg.textContent = result.message || 'Failed to update post';
            errorMsg.style.display = 'block';
        }

    } catch (error) {
        errorMsg.textContent = 'Failed to update. Are you logged in?';
        errorMsg.style.display = 'block';
        console.error(error);
    }
}

async function deletePost() {
    if (!confirm('Are you sure you want to delete this post?'))
        return;

    try {
        const result = await api.delete(`Post/${postId}/deletePost`);

        if (result.message === 'Post deleted successfully') {
            if (referrer && referrer.includes('Community')) {
                window.location.href = referrer;
            } else {
                window.location.href = 'Feed.html';
            }

        } else {
            const errorMsg = document.getElementById('errorMsg');
            errorMsg.textContent = result.message || 'Failed to delete post';
            errorMsg.style.display = 'block';
        }

    } catch (error) {
        console.error(error);
    }
}

window.updatePost = updatePost;
window.deletePost = deletePost;