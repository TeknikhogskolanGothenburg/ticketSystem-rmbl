// Write your JavaScript code.
$(document).ready(function () {
    var popoverTemplate = ['<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-header"></h3><div class="popover-body"></div></div>'].join('');
    var content1 = [`
        <div class="create-and-login">
        	<div class="CreateUser">
        		<div class="row">
        			<div class="col">
        				<p>Username</p>
        				<input type="text">
        			</div>
        			<div class="col">
        				<p>Email</p>
        				<input type="text">
        			</div>
        			<div class="col">
        				<p>Password</p>
        				<input type="password">
        			</div>
        		</div>
        		<div class="row">
        			<div class="col">
        				<p>First Name</p>
        				<input type="text">
        			</div>
        			<div class="col">
        				<p>Last Name</p>
        				<input type="text">
        			</div>
        		</div>
        		<div class="row">
        			<div class="col">
        				<p>City</p>
        				<input type="text">
        			</div>
        			<div class="col">
        				<p>ZipCode</p>
        				<input type="text">
        			</div>
        			<div class="col">
        				<p>Address</p>
        				<input type="text">
        			</div>
        			<div class="col" id="SubmitButton">
        				<button class="btn btn-primary text-right">Submit</button>
        			</div>
        		</div>
        	</div><!--Create User End--></div>` ].join('');
    var content2 = [`<div class="LogIn">
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
		</div><!--LogIn End-->`].join('');
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
});