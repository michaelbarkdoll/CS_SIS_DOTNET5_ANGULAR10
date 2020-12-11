namespace API.DTOs
{
    public class ClassListDto
    {
        /*
        Semester	FULL_COURSE_DESC	CRN	COURSE_TITLE	COLLEGE	DEPARTMENT_DESC	STUDENT_ID	
        STUDENT_NAME	INTERNET_ADDRESS	MAJOR	PROGRAM	CLASS_LEVEL_BOAP	BLDG_ROOM1	
        CLASS_MEETS1	BEGIN_TIME1	END_TIME1	BLDG_ROOM2	CLASS_MEETS2	BEGIN_TIME2	END_TIME2	
        BLDG_ROOM3	CLASS_MEETS3	BEGIN_TIME3	END_TIME3	COURSE_STATUS	PRIMARY_INSTR_NAME	
        COURSE_CREDITS	WDL_DROP_DATE	PREREQ	FINAL_GRADE	ASSIGNED_GRADE	REG_SEQ_NUM	MIDTERM_GRADE	
        Site
        */
        /*
        Spring 2021	CS200B 001	20939	Computer Concepts	Science	Computer Science	853135804	Almores, Marian Angela T.	marianangela.almores@siu.edu	MICR	SC-BS   MICR	SR	ONL 	R	1000	1050	ONL1 				 				Active	Rachamalla, Sruthi	3					20		CA
        */
        public string Semester { get; set; }
        public string FULL_COURSE_DESC { get; set; }
        public string CRN { get; set; }
        public string COURSE_TITLE { get; set; }
        public string COLLEGE { get; set; }
        public string DEPARTMENT_DESC { get; set; }
        public string STUDENT_ID { get; set; }
        public string STUDENT_NAME { get; set; }
        public string INTERNET_ADDRESS { get; set; }
        public string MAJOR { get; set; }
        public string PROGRAM { get; set; }
        public string CLASS_LEVEL_BOAP { get; set; }
        public string BLDG_ROOM1 { get; set; }
        public string CLASS_MEETS1 { get; set; }
        public string BEGIN_TIME1 { get; set; }
        public string END_TIME1 { get; set; }
        public string BLDG_ROOM2 { get; set; }
        public string CLASS_MEETS2 { get; set; }
        public string BEGIN_TIME2 { get; set; }
        public string END_TIME2 { get; set; }
        public string BLDG_ROOM3 { get; set; }
        public string CLASS_MEETS3 { get; set; }
        public string BEGIN_TIME3 { get; set; }
        public string END_TIME3 { get; set; }
        public string COURSE_STATUS { get; set; }
        public string PRIMARY_INSTR_NAME { get; set; }
        public string COURSE_CREDITS { get; set; }
        public string WDL_DROP_DATE { get; set; }
        public string PREREQ { get; set; }
        public string FINAL_GRADE { get; set; }
        public string ASSIGNED_GRADE { get; set; }
        public string REG_SEQ_NUM { get; set; }
        public string MIDTERM_GRADE { get; set; }
        public string Site { get; set; }

    }
}