   M S B A M L   `   `   `  ����I  EInitialProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:InitialProject.WPF.ViewsInitialProject.WPF.Views  5   	   1    +InitialProject.WPF.Views.TourStatisticsView   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:InitialProject.WPF.Views     5   	     � Title$  TourStatisticsView��5   	   $	��250��6$   $	��200��61    � AllowsTransparency$
 True��6=   
������  � WindowStyle$
 None=�6p    � 
ResizeMode$ NoResize=�6�    � WindowStartupLocation$ CenterScreen=�6�    � 	Resources 5	       �� MergedDictionaries �� Source�� 5
   
    5      �� 5      $' !../../Styles/GuideDictionary.xaml?�6%   %    5      .��5      �� ���	#FF1A1A1A   HighlightColorSolid#���   6$   $��1q�6X   ._�5   
   �  � RowDefinitions 5      �� 5      $	��89* �6    �� 5      $	��28* �6    .+�5      �� $��5,0,0,0q�6   .+�5      �� $��Number of guests: ��6   $��16(�65   $
��Bold"�6C   
�������6j   � 5      	 � ColumnDefinitions	 5      �� 5      $
��Auto �6+   �� 5       5      �� 5       �� 5!      �� 5"      �� 5#      �� 5$      .+�5(      �� $��0��6   $��0��6,   $��Younger than 18:��69   
�������6T   �� 5)      $��1��6   $��0��6,   
���������69   �� *YoungerThanEighteen+6a   �� 5+      $��0��6   $��1��6,   $��18 - 50:��69   
�������6L   �� 5,      $��1��6   $��1��6,   
���������69   �� *EighteenToFifty+6]   �� 5/      $��0��6   $��2��6,   $��Older than 50:��69   
�������6R   �� 50      $��1��6   $��2��6,   
���������69   �� *OlderThanFifty+6\   �� 53      $��0��6   $��3��6,   $��With voucher (%): ��69   
�������6V   �� 54      $��1��6   $��3��6,   
���������69   �� *WithVoucher+6Y   �� 57      $��0��6   $��4��6,   $��Without voucher (%): ��69   
�������6Y   �� 58      $��1��6   $��4��6,   
���������69   �� *WithoutVoucher+6\   59      5:      �� 5<      $��1��6     
MenuBorder#���  6"   ._�5=      �� -   5        $��Close��5=        BottomMenuButton#���  6)   $��Center=�6T   6q   5>      5?      5@      5A      