// Write your JavaScript code.
$(document).ready(function () {
    var popoverTemplate = [`
        <div class="popover" role= "tooltip">
            <div class="arrow"></div>
            <h3 class="popover-header">
            </h3> <div class="popover-body">
        </div>
        </div> `].join('');
    
    var content1 = [`
    @using(Html.BeginForm("RegisterUser", "Home") FormMethod.Post)
    {
        <div class="create-and-login">
        	<div class="CreateUser">
        		<div class="row">
        			<div class="col">
                <label>
                    Username:
                    @Html.EditorFor(m => m.Username)
                </label>
        			</div>
        			<div class="col">
                <label>
                    Email:
                    @Html.EditorFor(m => m.Email)
                </label>
        			</div>
        			<div class="col">
                <label>
                    New password:
                    @Html.EditorFor(m => m.Password)
                </label>
        			</div>
        		</div>
        		<div class="row">
        			<div class="col">
                <label>
                    First name:
                    @Html.EditorFor(m => m.FirstName)
                </label>
        			</div>
        			<div class="col">
                <label>
                    Last name:
                    @Html.EditorFor(m => m.LastName)
                </label>
        			</div>
        		</div>
        		<div class="row">
        			<div class="col">
                <label>
                    City:
                    @Html.EditorFor(m => m.City)
                </label>
        			</div>
        			<div class="col">
                <label>
                    Zip Code:
                    @Html.EditorFor(m => m.ZipCode)
                </label>
        			</div>
        			<div class="col">
                <label>
                    Address:
                    @Html.EditorFor(m => m.Address)
                </label>
        			</div>
        			<div class="col" id="SubmitButton">
        				<button class="btn btn-primary text-right">Submit</button>
        			</div>
        		</div>
        	</div><!--Create User End--></div>
    }`].join('');

    var content2 = [`
        <div class="LogIn">
			      <div class="row">
				        <div class="col">
					      <p>Username</p>
					      <input type="text">
				     </div>
				<div class="col">
					<p>Password</p>
					<input type="password">
				</div>
				<div class="col">
					<button class="btn btn-primary">Log In</button>
				</div>
			  </div>
		    </div>`].join('');

    $('.create-account-popover').popover({
        trigger: 'click',
        animation: true,
        content: content1,
        template: popoverTemplate,
        placement: "left",
        html: true
    });
    $('.log-in-popover').popover({
        trigger: 'click',
        animation: true,
        content: content2,
        template: popoverTemplate,
        placement: "left",
        html: true
    });
    $('#myList a').on('click', function (e) {
            e.preventDefault()
      $(this).tab('show');
    });
});