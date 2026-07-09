import { api } from '../api.js';
window.onload=()=>{
    var createcommunitybtn = document.getElementById("createcommunity-btn");
    var communitycontainer = document.getElementById("communities-container");
    var createcontainer = document.getElementById("creation-card");
    const errorMsg = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');
    
     createcommunitybtn.addEventListener('click',(e) => {
                        createCommunity(e.target);});

    async function GetCommunities() {
        try{
            const communities = await api.get('communities/getownedcommunities');
            if(!communities || communities.length === 0){
                communitycontainer.innerHTML = '<span style="color: Red;" class ="mt-2">No Created Communities.</span>';
                return; 
            }
            communitycontainer.innerHTML = '';
            communities.forEach(community => {
                const communityCard = document.createElement('div');
                communityCard.innerHTML = `
                <div class="row align-items-center justify-content-evenly community-card">
                <div class="col-md-3 text-center align-items-center mt-4">
                
               <img src="${community.iconURL ? community.iconURL : 'https://cdn-icons-png.freepik.com/512/11925/11925833.png'}" 
                alt="Community Icon" 
                class="profile-img">
                </div>

                <div class="col-md-7 mt-4">
                <h3 class="community-name  mb-1">
                <span id="name-display">${community.name}</span>
                <input type="text" id="name-input" class="form-control d-none" value="${community.name}">
                </h3>
                <p class="community-description" >
                <span id="desc-display">${community.description}</span>
                 <textarea id="desc-input" class="form-control d-none">${community.description}</textarea>
                 </p>
                 <p class="text-white-50" >
                <textarea id="url-input" class="form-control d-none" placeholder ="IconUrl(Optional)">${community.iconURL? community.iconURL: ''}</textarea>
                </p>
        
                <div class="d-flex gap-4"style="color:#bca1ec;">
                <small><strong>Creation Date: </strong>${new Date(community.dateCreated).toLocaleDateString()}</small>
                <small><strong>Members count: </strong>${community.memberscount}</small>
            </div>
            </div>
                <div class="actions m-3 col-12 text-center">
                    <button type="button" class="btn btn-outline-secondary update-btn" id ="update">Update Community</button>
                    <button type="button" class="btn btn-outline-secondary save-btn d-none" id ="save">Save Changes</button>
                    <button type="button" class="btn btn-outline-danger d-none" id= "cancel-update">Cancel</button>
                    <button type="button" class="btn btn-outline-danger delete-btn" id= "delete">Delete Community</button>
                </div>
            </div>`;
            communityCard.querySelector('.update-btn').addEventListener('click', (e) => {
                        Update_TextAreas(e.target);});
            communityCard.querySelector('.save-btn').addEventListener('click', (e) => {
                        UpdateCommunity(community.communityId, e.target);});

            communityCard.querySelector('#cancel-update').addEventListener('click', (e) => {
                    const card = e.target.closest('.community-card');

                    card.querySelector('#desc-input').classList.add('d-none');
                    card.querySelector('#desc-display').classList.remove('d-none');
                    card.querySelector('#name-input').classList.add('d-none');
                    card.querySelector('#name-display').classList.remove('d-none');
                    card.querySelector('#url-input').classList.add('d-none');

                    card.querySelector('.delete-btn').classList.remove('d-none');
                    card.querySelector('.save-btn').classList.add('d-none');
                    card.querySelector('.update-btn').classList.remove('d-none');
                    card.querySelector('#update').disabled = false;
                    card.querySelector('#cancel-update').classList.add('d-none'); 
            });
            communityCard.querySelector('.delete-btn').addEventListener('click', (e) => {
                        DeleteCommunity(community.communityId, e.target);});
            communitycontainer.appendChild(communityCard);
                
            });
        }
         catch(error){
           errorMsg.textContent = "Couldn't load communities: " + error.message;
            errorMsg.style.display = 'block';
        }
        
    }

    async function createCommunity(button) {
        button.classList.add('d-none');
        createcontainer.innerHTML =`
                <div class="row align-items-center community-card-form">
                <div class="col-md-9 text-center row align-items-center mt-4 ">
        
                <span class="text-light mb-1">
                <input type="text" id="newname-input" class="form-control mb-2" placeholder="Community Name">
                </span>
                <p class="text-white-50" >
                 <textarea id="newdesc-input" class="form-control mb-2" placeholder="Description(Optional)"></textarea>
                 </p>
                 <p class="text-white-50" >
                 <textarea id="newurl-input" class="form-control mb-2" placeholder="Icon URL(Optional)"></textarea>
                 </p>
            </div>
                <div class="actions m-3 col-12 text-center">
                    <button type="button" class="btn btn-outline-success create-btn" id ="create">Create Community</button>
                    <button type="button" class="btn btn-outline-danger cancel-btn" id= "cancel">Cancel</button>
                </div>
                  
            </div>`;
            createcontainer.querySelector('.create-btn').addEventListener('click', (e) => {
                        create(e.target);});
            createcontainer.querySelector('.cancel-btn').addEventListener('click', (e) => {
                       createcontainer.innerHTML = '';
                       button.classList.remove('d-none');
            });
    }

    GetCommunities();

    async function Update_TextAreas(buttonElement) {
        const card = buttonElement.closest('.community-card');
        buttonElement.disabled = true;

            card.querySelector('#desc-input').classList.remove('d-none');
            card.querySelector('#desc-display').classList.add('d-none');
            card.querySelector('#name-input').classList.remove('d-none');
            card.querySelector('#name-display').classList.add('d-none');
            card.querySelector('#url-input').classList.remove('d-none');

            card.querySelector('.delete-btn').classList.add('d-none');
            card.querySelector('.save-btn').classList.remove('d-none');
            card.querySelector('#cancel-update').classList.remove('d-none');
            buttonElement.classList.add('d-none');

    }

    async function UpdateCommunity(communityId ,buttonElement){
            buttonElement.disabled = true;
            const card = buttonElement.closest('.community-card');
            
            var namedisplay = card.querySelector('#name-display');
            var nameinput = card.querySelector('#name-input');
            var descdisplay = card.querySelector('#desc-display');
            var descinput = card.querySelector('#desc-input');
            var urlinput = card.querySelector('#url-input');
            const namevalue = nameinput.value;
            if(!namevalue){
             errorMsg.textContent = 'Please fill all required fields';
            errorMsg.style.display = 'block';
                return;
            }

            try{
                const result = await api.patch(`communities/${communityId}/updatecommunity`,{
                    description : descinput.value,
                    name : namevalue,
                    iconURL : urlinput.value

                });
                if(result.message ==="Community Updated Successfully"){

            namedisplay.textContent = nameinput.value;
            descdisplay.textContent = descinput.value;
            card.querySelector('.profile-img').src = urlinput.value? urlinput.value : 'https://cdn-icons-png.freepik.com/512/11925/11925833.png';
            namedisplay.classList.remove('d-none');
            nameinput.classList.add('d-none');
            urlinput.classList.add('d-none');
    
            descdisplay.classList.remove('d-none');
            descinput.classList.add('d-none');
                    
            card.querySelector('.delete-btn').classList.remove('d-none');
            card.querySelector('.save-btn').classList.add('d-none');
            card.querySelector('#cancel-update').classList.add('d-none');
            card.querySelector('.update-btn').classList.remove('d-none');

                     successMsg.textContent = 'Updated Community';
                     successMsg.style.display = 'block';
                    GetCommunities();
                }
            }
        catch(error){
       errorMsg.textContent = "Error: " + error.message;
            errorMsg.style.display = 'block';
        }

    }

    async function DeleteCommunity(communityId,buttonElement){
            buttonElement.disabled = true;
            try{
                const result = await api.delete(`communities/${communityId}/deletecommunity`);
                if(result.message ==="Community Deleted Successfully"){
                     successMsg.textContent = 'Community deleted';
                     successMsg.style.display = 'block';
                    GetCommunities();
                }
            }
        catch(error){
          errorMsg.textContent = "Error: " + error.message;
            errorMsg.style.display = 'block';
        }

    }

    async function create(buttonElement){   
        buttonElement.disabled = true;
        var nameinput = document.getElementById("newname-input");
        var descinput = document.getElementById("newdesc-input");
        var urlinput = document.getElementById("newurl-input");
        const namevalue = nameinput.value;
        if(!namevalue){
             errorMsg.textContent = 'Please fill all required fields';
            errorMsg.style.display = 'block';
                return;
            }

        try{
                const result = await api.post(`communities/createcommunity`,{
                    description : descinput.value,
                    name : namevalue,
                    iconURL : urlinput.value
                });
                if(result.message === "Community Created Successfully"){
                    var createcontainer = document.getElementById("creation-card");
                    createcontainer.innerHTML = '';
                    successMsg.textContent = 'Community created';
                     successMsg.style.display = 'block';
                    createcommunitybtn.classList.remove('d-none');
                    GetCommunities();
                }
            }
            catch(error){
                  errorMsg.textContent = "Error: " + error.message;
            errorMsg.style.display = 'block';
                buttonElement.disabled = false;
            }
    }
};

function goBack() {
    window.history.back();
}
window.goBack = goBack;