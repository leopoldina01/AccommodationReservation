   M S B A M L   `   `   `  ����I  EInitialProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:InitialProject.WPF.ViewsInitialProject.WPF.Views  5   	   8    2InitialProject.WPF.Views.AlternativeTourOffersView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:InitialProject.WPF.Views     5   	     N� Title$  Alternative Tours��5   	   $	��648��6#   $
��1024��60    N� 
Background
 �����.��5	      �  � RowDefinitions 5
   
   �� 5      $
��5.3* �6   �� 5      $��1* �6   .+�5   
   � 
�������$��0��6(   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ DataGridTours��5   $   $��1��6      SelectionMode$ Single=�6�      AutoGenerateColumns$ False��6�      
IsReadOnly$
 True��6  $��
10,30,10,0q�6$  ��6;   �� *AlternativeTours+ �� UpdateSourceTrigger$ PropertyChanged=� �� SelectedItem 6�   �� *AlternativeSelectedTour+$ PropertyChanged=�	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$

 Name��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ 143.3 67   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6E   �� *Name+  5      $
 Country��6)   $ 143.3 6:    6H   �� *Location.Country+  5      $

 City��6)   $ 143.3 67    6E   �� *Location.City+  5      $
 Language��6)   $ 143.3 6;    6I   �� *
Language+  5      $
 Max. Guests��6)   $ 143.3 6>    6L   �� *	MaxGuests+  5      $
 
Start Time��6)   $ 143.3 6=    6K   �� *	StartTime+  5      $
 Duration��6)   $	 120 6;    6G   �� *
Duration+5      5      � 5   
   $��1��6   
�������.+�5      �� $��Cancel��6   $
��Left=�6&   $��	58,50,0,0q�6A   $	��Top=�6T   $	��120��6l   $
��Bold"�6�   ��6x   �� *CancelCommand+6�   �� 5      -   5        $��ChooseButton��5      $��Choose��6)   $��Right=�6:   $��	0,50,58,0q�6V   $	��Top=�6i   $	��120��6�   $
��Bold"�6�   ��6�   �� *ChooseCommand+6�   5      5       5!      