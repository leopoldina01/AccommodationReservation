   M S B A M L   `   `   `  ����I  EInitialProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY1clr-namespace:InitialProject.WPF.Views.OwnerViews#InitialProject.WPF.Views.OwnerViews  5   	   5    /InitialProject.WPF.Views.OwnerViews.ForumWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Clocal1clr-namespace:InitialProject.WPF.Views.OwnerViews     5   	   -   5          � Title$  Location accommodations��5   	   $	��600��6)   $	��600��66    � WindowStartupLocation$ CenterScreen=�6B    J� InputBindings 5
      �� 5   
    �� Gesture$ CTRL+R��6>    �� Command 6   �� *ReportCommentCommand+6>   �� 5   
   $ ENTER��6<    6   �� *SendCommentCommand+6<    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $' !../../Styles/OwnerDictionary.xaml?�6%   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $
��300* �6   �� 5      $	��210 �6   �� 5      $��60 �6   .+�5   
   �� $��10q�6   $��0��6      ListBoxItem#��[  6+   	 �� SelectionMode$	 Single=�6�   ��6]   �� 
 �� Path$
 Comments� �� UpdateSourceTrigger$ PropertyChanged=� �� SelectedItem 6�   �� *SelectedComment+$ PropertyChanged=���5      �� 5      .R�5       � $	��130��6    � ColumnDefinitions 5!      �� 5"      $	��460 �6/   �� 5#      $	��10* �6/   .+�5%      �� $��0��6%   .+�5&      ��  �� Orientation$ Vertical=�6)   .+�5'   "   �� $
��Wrap=�6,    �� TextAlignment$
 Left=�6[   $��5,0,0,0q�6p     BoldTextBloxk#��[ 6�   ��6@   �� *	User.Role+6�   �� 5(   "   $
��Wrap=�6,   $
 Left=�6_   $��5,0,0,0q�6t     BasicTextBlock#��[ 6�   ��6@   �� *User.Username+6�   5)      �� 5*      $ 
Horizontal=�6)   .+�5+   "   �� $
��Wrap=�6,   $��Number of reports: ��6@   $
 Left=�6[   $��5,0,0,0q�6p   #��[ 6�   �� 5,   "   $
��Wrap=�6,   $
 Left=�6a   $��5,0,0,0q�6v   #��[ 6�   ��6@   �� *NumberOfReports+6�   5-      �� 5.      .+�5/   "   �� $
��Wrap=�6,   $��	Comment: ��6@   $
 Left=�6Q   $��5,0,0,0q�6f   #��[ 6w   �� 50   "   $
��Wrap=�6,   $
 Left=�6V   $��5,0,0,0q�6k   #��[ 6|   ��6@   �� *Text+6|   51      52      � 54      $��1��6   .+�55      �� $��Report��6%   $��Bottom=�66   $��0,0,0,20q�6Q     ListBoxButton#��[ 6d   ��6�   �� *" DataContext.ReportCommentCommand+ �� RelativeSource ��  �� AncestorType" ��56      58      59      5;      � 5=   
   $��1��6    5>      �� 5?      $��30 �6    �� 5@      $
��100* �6    �� 5A      $��60 �6     5C      �� 5D      $
��100* �6#   �� 5E      $	��120 �6#   .+�5H      �� -   5        $��ForumComment��5H   (   $��2��6   $��0��6<   $��Type your comment here:��6I   $
��Left=�6k   $��10,0,0,0q�6�     
BasicLabel#��[ 6�   �� 5J      -   5        $��TextBoxForumComment��5J   *   $��2��6   $��1��6�   $	��570��6�   $��0,0,0,0q�6�    �� TextWrapping$
 Wrap=�6�   " �� VerticalScrollBarVisibility$ Visible=�6�   $��Center=�6�   ��6E   �� *	Comment+$ PropertyChanged=�6�   �� 5L      $��2��6   $��0��6!     MessageLabelStyle#��[ 6w   ��61   �� *MessageLabel+$ PropertyChanged=�6w   �� 5N      -   5        $��
ButtonSend��5N   %   $��1��6   $��2��67   $
��Send��6D   $��Right=�6S   $��0,0,20,0q�6o     CloseButton#��[ 6�   ��6�   �� *SendCommentCommand+5O      � 5Q   
   $��2��6   .+�5R      �� -   5        $��Button_Close��5R      #��[ 6)   $��Right=�6N   $��0,0,20,0q�6j   $��Close��6|   6�   5S      5T      5U      