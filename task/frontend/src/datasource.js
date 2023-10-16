

export var projectData = [
    {
        'TaskID': 1,
        'TaskName': 'Parent Task 1',
        'StartDate': new Date('02/23/2017'),
        'Duration': 3,
        'Priority': 'Normal',
        'EndDate': new Date('02/27/2017'),
        'Progress': '40'
    },
    {
        'TaskID': 2,
        'TaskName': 'Child Task 1',
        'StartDate': new Date('02/23/2017'),
        'Duration': 4,
        'Priority': 'Low',
        'EndDate': new Date('02/27/2017'),
        'Progress': '40',
        'parentID': 1
    },
    {
        'TaskID': 3, 'TaskName': 'Child Task 2', 'StartDate': new Date('02/23/2017'), 'Duration': 2, 'Priority': 'Normal',
        'EndDate': new Date('02/27/2017'), 'Progress': '40', 'parentID': 1
    },
    {
        'TaskID': 4, 'TaskName': 'Child Task 3', 'StartDate': new Date('02/23/2017'), 'Duration': 2, 'Priority': 'Low',
        'EndDate': new Date('02/27/2017'), 'Progress': '40', 'parentID': 1
    },
    {
        'TaskID': 5, 'TaskName': 'Parent Task 2', 'StartDate': new Date('03/14/2017'), 'Duration': 6, 'Priority': 'Normal',
        'EndDate': new Date('03/18/2017'), 'Progress': '40'
    },
    {
        'TaskID': 6, 'TaskName': 'Child Task 1', 'StartDate': new Date('03/02/2017'), 'Duration': 11, 'Priority': 'High',
        'EndDate': new Date('03/06/2017'), 'Progress': '40', 'parentID': 5
    },
    {
        'TaskID': 7, 'TaskName': 'Child Task 2', 'StartDate': new Date('03/02/2017'), 'Duration': 7, 'Priority': 'Critical',
        'EndDate': new Date('03/06/2017'), 'Progress': '40', 'parentID': 5
    },
    {
        'TaskID': 8, 'TaskName': 'Child Task 3', 'StartDate': new Date('03/02/2017'), 'Duration': 10, 'Priority': 'Breaker',
        'EndDate': new Date('03/06/2017'), 'Progress': '40', 'parentID': 5
    },
    {
        'TaskID': 9, 'TaskName': 'Child Task 4', 'StartDate': new Date('03/02/2017'), 'Duration': 15, 'Priority': 'High',
        'EndDate': new Date('03/06/2017'), 'Progress': '40', 'parentID': 5
    },
    {
        'TaskID': 10, 'TaskName': 'Parent Task 3', 'StartDate': new Date('03/09/2017'), 'Duration': 17, 'Priority': 'Breaker',
        'EndDate': new Date('03/13/2017'), 'Progress': '40'
    },
    {
        'TaskID': 11, 'TaskName': 'Child Task 1', 'StartDate': new Date('03/9/2017'), 'Duration': 0, 'Priority': 'Low',
        'EndDate': new Date('03/13/2017'), 'Progress': '40', 'parentID': 10
    },
    {
        'TaskID': 12, 'TaskName': 'Child Task 2', 'StartDate': new Date('03/9/2017'), 'Duration': 10, 'Priority': 'Breaker',
        'EndDate': new Date('03/13/2017'), 'Progress': '40', 'parentID': 10
    },
    {
        'TaskID': 13, 'TaskName': 'Child Task 3', 'StartDate': new Date('03/9/2017'), 'Duration': 11, 'Priority': 'Normal',
        'EndDate': new Date('03/13/2017'), 'Progress': '40', 'parentID': 10
    },
    {
        'TaskID': 14, 'TaskName': 'Child Task 4', 'StartDate': new Date('03/9/2017'), 'Duration': 1, 'Priority': 'Normal',
        'EndDate': new Date('03/13/2017'), 'Progress': '40', 'parentID': 10
    },
    {
        'TaskID': 15, 'TaskName': 'Child Task 5', 'StartDate': new Date('03/9/2017'), 'Duration': 14, 'Priority': 'Critical',
        'EndDate': new Date('03/13/2017'), 'Progress': '40', 'parentID': 10
    }
];
export var kanbanData = [
    {
        'Id': 1,
        'Status': 'Open',
        'Summary': 'Analyze the new requirements gathered from the customer.',
        'Type': 'Story',
        'Priority': 'Low',
        'Tags': 'Analyze,Customer',
        'Estimate': 3.5,
        'Assignee': 'Nancy Davloio',
        'RankId': 1
    },
    {
        'Id': 2,
        'Status': 'InProgress',
        'Summary': 'Improve application performance',
        'Type': 'Improvement',
        'Priority': 'Normal',
        'Tags': 'Improvement',
        'Estimate': 6,
        'Assignee': 'Andrew Fuller',
        'RankId': 1
    },
    {
        'Id': 3,
        'Status': 'Open',
        'Summary': 'Arrange a web meeting with the customer to get new requirements.',
        'Type': 'Others',
        'Priority': 'Critical',
        'Tags': 'Meeting',
        'Estimate': 5.5,
        'Assignee': 'Janet Leverling',
        'RankId': 2
    },
    {
        'Id': 4,
        'Status': 'InProgress',
        'Summary': 'Fix the issues reported in the IE browser.',
        'Type': 'Bug',
        'Priority': 'Release Breaker',
        'Tags': 'IE',
        'Estimate': 2.5,
        'Assignee': 'Janet Leverling',
        'RankId': 2
    },
    {
        'Id': 5,
        'Status': 'Testing',
        'Summary': 'Fix the issues reported by the customer.',
        'Type': 'Bug',
        'Priority': 'Low',
        'Tags': 'Customer',
        'Estimate': '3.5',
        'Assignee': 'Steven walker',
        'RankId': 1
    },
    {
        'Id': 6,
        'Status': 'Close',
        'Summary': 'Arrange a web meeting with the customer to get the login page requirements.',
        'Type': 'Others',
        'Priority': 'Low',
        'Tags': 'Meeting',
        'Estimate': 2,
        'Assignee': 'Michael Suyama',
        'RankId': 1
    },
    {
        'Id': 7,
        'Status': 'Validate',
        'Summary': 'Validate new requirements',
        'Type': 'Improvement',
        'Priority': 'Low',
        'Tags': 'Validation',
        'Estimate': 1.5,
        'Assignee': 'Robert King',
        'RankId': 1
    },
    {
        'Id': 8,
        'Status': 'Close',
        'Summary': 'Login page validation',
        'Type': 'Story',
        'Priority': 'Release Breaker',
        'Tags': 'Validation,Fix',
        'Estimate': 2.5,
        'Assignee': 'Laura Callahan',
        'RankId': 2
    },
    {
        'Id': 9,
        'Status': 'Testing',
        'Summary': 'Fix the issues reported in Safari browser.',
        'Type': 'Bug',
        'Priority': 'Release Breaker',
        'Tags': 'Fix,Safari',
        'Estimate': 1.5,
        'Assignee': 'Nancy Davloio',
        'RankId': 2
    },
    {
        'Id': 10,
        'Status': 'Close',
        'Summary': 'Test the application in the IE browser.',
        'Type': 'Story',
        'Priority': 'Low',
        'Tags': 'Testing,IE',
        'Estimate': 5.5,
        'Assignee': 'Margaret hamilt',
        'RankId': 3
    },
    {
        'Id': 11,
        'Status': 'Validate',
        'Summary': 'Validate the issues reported by the customer.',
        'Type': 'Story',
        'Priority': 'High',
        'Tags': 'Validation,Fix',
        'Estimate': 1,
        'Assignee': 'Steven walker',
        'RankId': 1
    },
    {
        'Id': 12,
        'Status': 'Testing',
        'Summary': 'Check Login page validation.',
        'Type': 'Story',
        'Priority': 'Release Breaker',
        'Tags': 'Testing',
        'Estimate': 0.5,
        'Assignee': 'Michael Suyama',
        'RankId': 3
    },
    {
        'Id': 13,
        'Status': 'Open',
        'Summary': 'API improvements.',
        'Type': 'Improvement',
        'Priority': 'High',
        'Tags': 'Grid,API',
        'Estimate': 3.5,
        'Assignee': 'Robert King',
        'RankId': 3
    },
    {
        'Id': 14,
        'Status': 'InProgress',
        'Summary': 'Add responsive support to application',
        'Type': 'Epic',
        'Priority': 'Critical',
        'Tags': 'Responsive',
        'Estimate': 6,
        'Assignee': 'Laura Callahan',
        'RankId': 3
    },
    {
        'Id': 15,
        'Status': 'Open',
        'Summary': 'Show the retrieved data from the server in grid control.',
        'Type': 'Story',
        'Priority': 'High',
        'Tags': 'Database,SQL',
        'Estimate': 5.5,
        'Assignee': 'Margaret hamilt',
        'RankId': 4
    },
    {
        'Id': 16,
        'Status': 'InProgress',
        'Summary': 'Fix cannot open user’s default database SQL error.',
        'Priority': 'Critical',
        'Type': 'Bug',
        'Tags': 'Database,Sql2008',
        'Estimate': 2.5,
        'Assignee': 'Janet Leverling',
        'RankId': 4
    },
    {
        'Id': 17,
        'Status': 'Testing',
        'Summary': 'Fix the issues reported in data binding.',
        'Type': 'Story',
        'Priority': 'Normal',
        'Tags': 'Databinding',
        'Estimate': '3.5',
        'Assignee': 'Janet Leverling',
        'RankId': 4
    },
    {
        'Id': 18,
        'Status': 'Close',
        'Summary': 'Analyze SQL server 2008 connection.',
        'Type': 'Story',
        'Priority': 'Release Breaker',
        'Tags': 'Grid,Sql',
        'Estimate': 2,
        'Assignee': 'Andrew Fuller',
        'RankId': 4
    },
    {
        'Id': 19,
        'Status': 'Validate',
        'Summary': 'Validate databinding issues.',
        'Type': 'Story',
        'Priority': 'Low',
        'Tags': 'Validation',
        'Estimate': 1.5,
        'Assignee': 'Margaret hamilt',
        'RankId': 1
    },
    {
        'Id': 20,
        'Status': 'Close',
        'Summary': 'Analyze grid control.',
        'Type': 'Story',
        'Priority': 'High',
        'Tags': 'Analyze',
        'Estimate': 2.5,
        'Assignee': 'Margaret hamilt',
        'RankId': 5
    },
    {
        'Id': 21,
        'Status': 'Close',
        'Summary': 'Stored procedure for initial data binding of the grid.',
        'Type': 'Others',
        'Priority': 'Release Breaker',
        'Tags': 'Databinding',
        'Estimate': 1.5,
        'Assignee': 'Steven walker',
        'RankId': 6
    },
    {
        'Id': 22,
        'Status': 'Close',
        'Summary': 'Analyze stored procedures.',
        'Type': 'Story',
        'Priority': 'Release Breaker',
        'Tags': 'Procedures',
        'Estimate': 5.5,
        'Assignee': 'Janet Leverling',
        'RankId': 7
    },
    {
        'Id': 23,
        'Status': 'Validate',
        'Summary': 'Validate editing issues.',
        'Type': 'Story',
        'Priority': 'Critical',
        'Tags': 'Editing',
        'Estimate': 1,
        'Assignee': 'Nancy Davloio',
        'RankId': 1
    },
    {
        'Id': 24,
        'Status': 'Testing',
        'Summary': 'Test editing functionality.',
        'Type': 'Story',
        'Priority': 'Normal',
        'Tags': 'Editing,Test',
        'Estimate': 0.5,
        'Assignee': 'Nancy Davloio',
        'RankId': 5
    },
    {
        'Id': 25,
        'Status': 'Open',
        'Summary': 'Enhance editing functionality.',
        'Type': 'Improvement',
        'Priority': 'Low',
        'Tags': 'Editing',
        'Estimate': 3.5,
        'Assignee': 'Andrew Fuller',
        'RankId': 5
    }
];
export var ganttData = [
    {
        TaskID: 1,
        TaskName: 'Project Initiation',
        StartDate: new Date('04/02/2019'),
        EndDate: new Date('04/21/2019'),
        subtasks: [
            { TaskID: 2, TaskName: 'Identify Site location', StartDate: new Date('04/02/2019'), Duration: 4, Progress: 50 },
            { TaskID: 3, TaskName: 'Perform Soil test', StartDate: new Date('04/02/2019'), Duration: 4, Progress: 50 },
            { TaskID: 4, TaskName: 'Soil test approval', StartDate: new Date('04/02/2019'), Duration: 4, Progress: 50 },
        ]
    },
    {
        TaskID: 5,
        TaskName: 'Project Estimation',
        StartDate: new Date('04/02/2019'),
        EndDate: new Date('04/21/2019'),
        subtasks: [
            { TaskID: 6, TaskName: 'Develop floor plan for estimation', StartDate: new Date('04/04/2019'), Duration: 3, Progress: 50 },
            { TaskID: 7, TaskName: 'List materials', StartDate: new Date('04/04/2019'), Duration: 3, Progress: 50 },
            { TaskID: 8, TaskName: 'Estimation approval', StartDate: new Date('04/04/2019'), Duration: 3, Progress: 50 }
        ]
    },
]
export var editingResources = [
    { resourceId: 1, resourceName: 'Martin Tamer' },
    { resourceId: 2, resourceName: 'Rose Fuller' },
    { resourceId: 3, resourceName: 'Margaret Buchanan' },
    { resourceId: 4, resourceName: 'Fuller King' },
    { resourceId: 5, resourceName: 'Davolio Fuller' },
    { resourceId: 6, resourceName: 'Van Jack' },
    { resourceId: 7, resourceName: 'Fuller Buchanan' },
    { resourceId: 8, resourceName: 'Jack Davolio' },
    { resourceId: 9, resourceName: 'Tamer Vinet' },
    { resourceId: 10, resourceName: 'Vinet Fuller' },
    { resourceId: 11, resourceName: 'Bergs Anton' },
    { resourceId: 12, resourceName: 'Construction Supervisor' }
];
export var templateData = [
    {
        TaskID: 1,
        TaskName: 'Product concept',
        StartDate: new Date('04/02/2019'),
        EndDate: new Date('04/21/2019'),
        subtasks: [
            {
                TaskID: 2, TaskName: 'Defining the product and its usage', StartDate: new Date('04/02/2019'),
                Duration: 3, Progress: 30, resources: [2]
            },
            {
                TaskID: 3, TaskName: 'Defining target audience', StartDate: new Date('04/02/2019'),
                Duration: 3, resources: [3]
            },
            {
                TaskID: 4, TaskName: 'Prepare product sketch and notes', StartDate: new Date('04/02/2019'),
                Duration: 2, Predecessor: '2', Progress: 30, resources: [4]
            }
        ]
    },
    {
        TaskID: 5, TaskName: 'Concept approval', StartDate: new Date('04/02/2019'), Duration: 0, Predecessor: '3,4', resources: [1]
    },
    {
        TaskID: 6,
        TaskName: 'Market research',
        StartDate: new Date('04/02/2019'),
        EndDate: new Date('04/21/2019'),
        subtasks: [
            {
                TaskID: 7,
                TaskName: 'Demand analysis',
                StartDate: new Date('04/04/2019'),
                EndDate: new Date('04/21/2019'),
                subtasks: [
                    {
                        TaskID: 8, TaskName: 'Customer strength', StartDate: new Date('04/04/2019'),
                        Duration: 4, Predecessor: '5', Progress: 30, resources: [5]
                    },
                    {
                        TaskID: 9, TaskName: 'Market opportunity analysis', StartDate: new Date('04/04/2019'),
                        Duration: 4, Predecessor: '5', resources: [6]
                    }
                ]
            },
            {
                TaskID: 10, TaskName: 'Competitor analysis', StartDate: new Date('04/04/2019'),
                Duration: 4, Predecessor: '7, 8', Progress: 30, resources: [4]
            },
            {
                TaskID: 11, TaskName: 'Product strength analsysis', StartDate: new Date('04/04/2019'),
                Duration: 4, Predecessor: '9', resources: [8]
            },
        ]
    }
];