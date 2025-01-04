<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PlatonStudentApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <!-- Hero Section -->
        <section class="jumbotron text-center">
            <h1 class="display-4">Welcome to the Student Registration System</h1>
            <p class="lead">Streamline student management and course enrollment effortlessly!</p>
            <p>
                <a href="Login.aspx" class="btn btn-primary btn-lg">Login &raquo;</a>
                <a href="Register.aspx" class="btn btn-secondary btn-lg">Register as Student &raquo;</a>
            </p>
        </section>

        <!-- Features Section -->
        <section class="container">
            <div class="row">
                <!-- Admin Dashboard Section -->
                <div class="col-md-4">
                    <div class="card">
                        <div class="card-header text-white bg-primary">
                            <h4>Admin Dashboard</h4>
                        </div>
                        <div class="card-body">
                            <p>Manage students, view enrollment details, and maintain system functionality.</p>
                            <p>
                                <a class="btn btn-primary" href="AdminDashboard.aspx">Go to Admin Dashboard &raquo;</a>
                            </p>
                        </div>
                    </div>
                </div>

                <!-- Student Dashboard Section -->
                <div class="col-md-4">
                    <div class="card">
                        <div class="card-header text-white bg-success">
                            <h4>Student Dashboard</h4>
                        </div>
                        <div class="card-body">
                            <p>Log in to enroll in courses, view your profile, and track academic progress.</p>
                            <p>
                                <a class="btn btn-success" href="StudentDashboard.aspx">Go to Student Dashboard &raquo;</a>
                            </p>
                        </div>
                    </div>
                </div>

                <!-- Course Management Placeholder -->
                <div class="col-md-4">
                    <div class="card">
                        <div class="card-header text-white bg-info">
                            <h4>Course Management</h4>
                        </div>
                        <div class="card-body">
                            <p>Coming Soon: Add, update, and manage courses in the system!</p>
                            <p>
                                <a class="btn btn-info" href="#">Coming Soon &raquo;</a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </main>

</asp:Content>
