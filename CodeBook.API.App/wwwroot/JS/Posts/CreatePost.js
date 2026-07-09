import { api } from '../api.js';

const urlParams = new URLSearchParams(window.location.search);
const communityId = urlParams.get('communityId');

document.addEventListener('DOMContentLoaded', () => {
    if (communityId) {
        document.getElementById('communityInfo').style.display = 'block';
        document.getElementById('communityId').value = communityId;
        loadCommunityName(communityId);

        document.getElementById('isPublic').checked = true;
    }
});

async function createPost() {
    const title = document.getElementById('title').value.trim();
    const body = document.getElementById('body').value.trim();
    const codeSnippet = document.getElementById('codeSnippet').value.trim();
    const language = document.getElementById('language').value;
    const isPublic = document.getElementById('isPublic').checked;
    const communityIdValue = document.getElementById('communityId').value;

    const hashtagMatches = body.match(/#(\w+)/g) || [];
    const hashtags = hashtagMatches.map(tag => tag.slice(1));

    const errorMsg = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');

    // hide previous messages
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
        const tagIds = await resolveTagIds(hashtags);

        const result = await api.post('Post/create', {
            title: title,
            body: body,
            codeSnippet: codeSnippet || null,
            language: language || null,
            isPublic: isPublic,
            communityId: communityIdValue ? parseInt(communityIdValue) : null,
            tagIds: tagIds
        });

        if (result.message === 'Post created successfully') {
            successMsg.textContent = 'Post created successfully!';
            successMsg.style.display = 'block';

            // redirect to feed after 3 seconds
            setTimeout(() => {
               if(communityIdValue){
                window.location.href = `../Community/CommunityFeed.html?id=${communityIdValue}`;
               }
               else{
                window.location.href = 'Feed.html';
               }
            }, 3000);
        } else {
            errorMsg.textContent = result.message || 'Failed to create post';
            errorMsg.style.display = 'block';
        }

    } catch (error) {
        errorMsg.textContent = 'Failed to create post. Are you logged in?';
        errorMsg.style.display = 'block';
        console.error(error);
    }
}
window.createPost=createPost;

async function resolveTagIds(hashtags) {
    if (!hashtags.length) return [];

    try {
        const result = await api.post('Tag/resolve', { tags: hashtags });
        return result.tagIds || [];
    } catch (error) {
        console.error('Failed to resolve tags:', error);
        return [];
    }
}

async function loadCommunityName(communityId) {
    try {
        const community = await api.get(`Communities/${communityId}`);
        if (community) {
            document.getElementById('communityName').textContent = community.name;
        }
    } catch (error) {
        console.error('Failed to load community:', error);
    }
}

window.createPost = createPost;