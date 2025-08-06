namespace LaborExchangeApi.Common
{
    public static class ValidationConstants
    {
        // Category
        public const int MAX_CATEGORY_NAME_LENGTH = 128;

        // Company
        public const int MAX_COMPANY_NAME_LENGTH = 128;

        // Resume
        public const int MAX_RESUME_TITLE_LENGTH = 128;
        public const int MAX_RESUME_SKILLS_LENGTH = 256;

        // Unemployed
        public const int MAX_UNEMPLOYED_FIRST_NAME_LENGTH = 128;
        public const int MAX_UNEMPLOYED_LAST_NAME_LENGTH = 128;
        public const int MAX_UNEMPLOYED_AGE = 120;

        // Vacancy
        public const int MAX_VACANCY_TITLE_LENGTH = 128;
        public const int MAX_VACANCY_DESCRIPTION_LENGTH = 512;
    }
}