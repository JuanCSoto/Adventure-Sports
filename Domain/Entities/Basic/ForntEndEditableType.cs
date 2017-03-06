// <copyright file="ForntEndEditableType.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.Basic
{
    /// <summary>
    /// enumeration for the editable content ,estos codigos pertenecen a la tabla FrontEnd
    /// </summary>
    public enum ForntEndEditableType
    {
        /// <summary>
        /// name for the email notification sender
        /// </summary>
        ADMIN_EMAIL_NAME = -1,

        /// <summary>
        /// value for sending the finishing process at N day to finish
        /// </summary>
        ADMIN_N_FINISHING_PROCESS = -2,

        /// <summary>
        /// site logo
        /// </summary>
        LOGO = 1,

        /// <summary>
        /// text home menu 
        /// </summary>
        MENU_HOME = 2,

        /// <summary>
        /// text article menu
        /// </summary>
        MENU_ARTICLES = 3,

        /// <summary>
        /// text FAQ menu
        /// </summary>
        MENU_FAQ = 4,

        /// <summary>
        /// text allies menu
        /// </summary>
        MENU_ALLIES = 29,

        /// <summary>
        /// text statistics menu
        /// </summary>
        MENU_STATISTICS = 30,

        /// <summary>
        /// text SuccessStory menu
        /// </summary>
        MENU_SUCCESSSTORY = 63,

        /// <summary>
        /// text Postula menu
        /// </summary>
        MENU_HOMEHTML = 65,

        /// <summary>
        /// text statistics menu
        /// </summary>
        MENU_POSTULA = 64,

        /// <summary>
        /// text community menu
        /// </summary>
        MENU_COMMUNITY = 31,

        /// <summary>
        /// image header background
        /// </summary>
        HEADER_IMAGE = 5,

        /// <summary>
        /// header small text
        /// </summary>
        HEADER_SMALL = 6,

        /// <summary>
        /// header big text
        /// </summary>
        HEADER_BIG = 7,

        /// <summary>
        /// header idea text
        /// </summary>
        HEADER_IDEAS = 8,

        /// <summary>
        /// header color
        /// </summary>
        HEADER_COLOR = 9,

        /// <summary>
        /// site background color
        /// </summary>
        BACKGROUND_COLOR = 10,

        /// <summary>
        /// pulses description
        /// </summary>
        PULSES_DESCRIPTION = 11,

        /// <summary>
        /// pulses tooltip
        /// </summary>
        PULSES_TOOLTIP = 12,

        /// <summary>
        /// statistics user top text
        /// </summary>
        STATISTICS_USER_TOP = 13,

        /// <summary>
        /// statistics idea top text
        /// </summary>
        STATISTICS_IDEA_TOP = 14,

        /// <summary>
        /// community text 1
        /// </summary>
        COMMUNITY_TEXT_1 = 15,

        /// <summary>
        /// community text 2
        /// </summary>
        COMMUNITY_TEXT_2 = 16,

        /// <summary>
        /// community description
        /// </summary>
        COMMUNITY_DESCRIPTION = 17,

        /// <summary>
        /// ideas created
        /// </summary>
        STATISTICS_IDEAS_CREATED = 18,

        /// <summary>
        /// challenges description
        /// </summary>
        CHALLENGES_DESCRIPTION = 19,

        /// <summary>
        /// questions description
        /// </summary>
        QUESTIONS_DESCRIPTION = 20,

        /// <summary>
        /// challenges singular
        /// </summary>
        CHALLENGES_SINGULAR = 21,

        /// <summary>
        /// questions singular
        /// </summary>
        QUESTIONS_SINGULAR = 22,

        /// <summary>
        /// challenges plural
        /// </summary>
        CHALLENGES_PLURAL = 23,

        /// <summary>
        /// questions plural
        /// </summary>
        QUESTIONS_PLURAL = 24,

        /// <summary>
        /// articles singular
        /// </summary>
        ARTICLES_SINGULAR = 25,

        /// <summary>
        /// articles plural
        /// </summary>
        ARTICLES_PLURAL = 26,

        /// <summary>
        /// pulses singular
        /// </summary>
        PULSES_SINGULAR = 27,

        /// <summary>
        /// pulses plural
        /// </summary>
        PULSES_PLURAL = 28,

        /// <summary>
        /// footer owner text
        /// </summary>
        FOOTER_OWNER = 32,

        /// <summary>
        /// footer owner logo
        /// </summary>
        FOOTER_OWNER_LOGO = 33,

        /// <summary>
        /// footer owner link
        /// </summary>
        FOOTER_OWNER_LINK = 34,

        /// <summary>
        /// footer sponsor name
        /// </summary>
        FOOTER_SPONSOR = 35,

        /// <summary>
        /// footer sponsor logo
        /// </summary>
        FOOTER_SPONSOR_LOGO_1 = 36,

        /// <summary>
        /// footer sponsor link
        /// </summary>
        FOOTER_SPONSOR_LINK_1 = 37,

        /// <summary>
        /// footer sponsor logo
        /// </summary>
        FOOTER_SPONSOR_LOGO_2 = 38,

        /// <summary>
        /// footer sponsor link
        /// </summary>
        FOOTER_SPONSOR_LINK_2 = 39,

        /// <summary>
        /// footer sponsor logo
        /// </summary>
        FOOTER_SPONSOR_LOGO_3 = 40,

        /// <summary>
        /// footer sponsor link
        /// </summary>
        FOOTER_SPONSOR_LINK_3 = 41,

        /// <summary>
        /// footer sponsor logo
        /// </summary>
        FOOTER_SPONSOR_LOGO_4 = 42,

        /// <summary>
        /// footer sponsor link
        /// </summary>
        FOOTER_SPONSOR_LINK_4 = 43,

        /// <summary>
        /// footer sponsor logo
        /// </summary>
        FOOTER_SPONSOR_LOGO_5 = 44,

        /// <summary>
        /// footer sponsor link
        /// </summary>
        FOOTER_SPONSOR_LINK_5 = 45,

        /// <summary>
        /// footer sponsor logo
        /// </summary>
        FOOTER_SPONSOR_LOGO_6 = 46,

        /// <summary>
        /// footer sponsor link
        /// </summary>
        FOOTER_SPONSOR_LINK_6 = 47,

        /// <summary>
        /// footer contact us text
        /// </summary>
        FOOTER_CONTACT_US = 48,

        /// <summary>
        /// footer policy text
        /// </summary>
        FOOTER_POLICY = 49,

        /// <summary>
        /// footer terms text
        /// </summary>
        FOOTER_TERMS = 50,

        /// <summary>
        /// button create idea text
        /// </summary>
        BUTTON_CREATE_IDEA = 51,

        /// <summary>
        /// pulse versus text
        /// </summary>
        PULSE_VS = 52,

        /// <summary>
        /// pulse idea top text
        /// </summary>
        PULSE_TOP = 53,

        /// <summary>
        /// button follow pulse text
        /// </summary>
        BUTTON_FOLLOW_PULSE = 54,

        /// <summary>
        /// users singular
        /// </summary>
        USERS_SINGULAR = 55,

        /// <summary>
        /// users plural
        /// </summary>
        USERS_PLURAL = 56,

        /// <summary>
        /// users description
        /// </summary>
        USERS_DESCRIPTION = 57,

        /// <summary>
        /// users tooltip
        /// </summary>
        USERS_TOOLTIP = 58,

        /// <summary>
        /// ideas singular
        /// </summary>
        IDEAS_SINGULAR = 59,

        /// <summary>
        /// ideas plural
        /// </summary>
        IDEAS_PLURAL = 60,

        /// <summary>
        /// ideas description
        /// </summary>
        IDEAS_DESCRIPTION = 61,

        /// <summary>
        /// ideas tooltip
        /// </summary>
        IDEAS_TOOLTIP = 62
    }
}
