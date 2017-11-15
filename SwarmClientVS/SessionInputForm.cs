﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SwarmClientVS.DataLog.FileLog;
using SwarmClientVS.Domain.IRepository;
using SwarmClientVS.Domain.Service;

namespace SwarmClientVS
{
    public partial class SessionInputForm : Form
    {
        private SessionService SessionService;
        private SessionInputService SessionInputService;
        private SessionListBoxItemModel NewProject;
        private SessionInputModel SessionInputModel;
        private bool CEEtxtProjectTitle = false;
        private bool CEEtxtProjectDescription = false;
        private bool CEElstProject = false;
        //CEE - can execute event - flag to blog events triggers by backend, allowing only trigger by user

        public SessionInputForm(SessionService sessionService, string solutionName)
        {
            InitializeComponent();

            SessionService = sessionService;
            SessionInputService = new SessionInputService(new RepositoryLog(), solutionName);

            LoadInputData();
        }

        private void LoadInputData()
        {
            DisableEvents();

            SessionInputModel = SessionInputService.GetInputDataState();

            lstProject.DataSource = SessionInputModel.Project;
            lstProject.DisplayMember = "Name";
            lstTask.ClearSelected();
            lstProject.SelectedItem = SessionInputModel.SelectedProject;

            txtProjectTitle.Text = SessionInputModel.SelectedProject.Name;
            txtProjectDescription.Text = SessionInputModel.SelectedProject.Description;

            lstTask.DataSource = SessionInputModel.SelectedProject.Task;
            lstTask.DisplayMember = "Name";
            lstTask.ClearSelected();
            lstTask.SelectedItem = SessionInputModel.SelectedTask;

            txtTaskTitle.Text = SessionInputModel.SelectedTask.Name;
            txtTaskDescription.Text = SessionInputModel.SelectedTask.Description;

            txtDeveloper.Text = SessionInputModel.Developer;

            EnableEvents();
        }

        private void UpdateProjectListBox()
        {
            DisableEvents();

            lstProject.DataSource = null;
            lstProject.DataSource = SessionInputModel.Project;
            lstProject.DisplayMember = "Name";
            lstProject.ClearSelected();
            lstProject.SelectedItem = SessionInputModel.SelectedProject;

            EnableEvents();
        }

        private void ChangeSelectedProject()
        {
            DisableEvents();

            txtProjectTitle.Text = SessionInputModel.SelectedProject.Name;
            txtProjectDescription.Text = SessionInputModel.SelectedProject.Description;

            txtTaskTitle.Text = String.Empty;
            txtTaskDescription.Text = String.Empty;

            lstTask.DataSource = SessionInputModel.SelectedProject.Task;
            lstTask.DisplayMember = "Name";
            lstTask.ClearSelected();
            lstTask.SelectedItem = SessionInputModel.SelectedProject.Task.LastOrDefault() ?? new SessionListBoxItemModel { };

            EnableEvents();
        }

        private void EnableEvents()
        {
            CEElstProject = true;
            CEEtxtProjectTitle = true;
            CEEtxtProjectDescription = true;
        }

        private void DisableEvents()
        {
            CEElstProject = false;
            CEEtxtProjectTitle = false;
            CEEtxtProjectDescription = false;
        }

        private void lstProject_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CEElstProject == false)
                return;

            if (lstProject.SelectedItem == null)
                return;

            SessionInputModel.SelectedProject = (SessionListBoxItemModel)lstProject.SelectedItem;

            ChangeSelectedProject();
        }

        private void lstTask_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lstTask.SelectedItem == null)
                return;

            txtTaskTitle.Text = ((SessionListBoxItemModel)lstTask.SelectedItem).Name;
            txtTaskDescription.Text = ((SessionListBoxItemModel)lstTask.SelectedItem).Description;
        }

        private void txtProjectTitle_TextChanged(object sender, EventArgs e)
        {
            if (CEEtxtProjectTitle == false)
                return;

            SessionListBoxItemModel existentProjectItem = SessionInputModel.Project.FirstOrDefault(p => p.Name.Equals(txtProjectTitle.Text));

            if (existentProjectItem != null)
            {
                if (existentProjectItem != NewProject)
                {
                    if (NewProject != null)
                    {
                        SessionInputModel.Project.Remove(NewProject);
                        NewProject = null;
                    }

                    SessionInputModel.SelectedProject = existentProjectItem;
                }
            }
            else
            {
                if (NewProject == null)
                {
                    NewProject = new SessionListBoxItemModel { Name = txtProjectTitle.Text };
                    SessionInputModel.Project.Add(NewProject);
                    SessionInputModel.SelectedProject = SessionInputModel.Project.Last();
                }

                NewProject.Name = txtProjectTitle.Text;
            }

            UpdateProjectListBox();
            ChangeSelectedProject();
        }

        private void txtProjectDescription_TextChanged(object sender, EventArgs e)
        {
            if (CEEtxtProjectDescription == false)
                return;

            SessionListBoxItemModel existentProjectItem = SessionInputModel.Project.FirstOrDefault(p => p.Name.Equals(txtProjectTitle.Text));

            if (existentProjectItem == null)
                return;

            existentProjectItem.Description = txtProjectDescription.Text;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SessionService.RegisterSessionInformation(new SessionModel
            {
                Project = txtProjectTitle.Text,
                ProjectDescription = txtProjectDescription.Text,
                Task = txtTaskTitle.Text,
                TaskDescription = txtTaskDescription.Text,
                Developer = txtDeveloper.Text
            });

            SessionInputService.PersistInputDataState(new SessionInputModel
            {
                Project = lstProject.Items.Cast<SessionListBoxItemModel>().ToList(),
                Developer = txtDeveloper.Text
            });

            Close();
        }
    }
}
