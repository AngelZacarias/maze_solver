   M S B A M L   `   `   `  ����F  Bmaze_solver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:maze_solvermaze_solver  5   	       maze_solver.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:maze_solver     5   	     � Title$  Maze solver��5   	   $	��480��6   $	��800��6*    � WindowStartupLocation$ CenterScreen=�66    � WindowStyle$
 None=�6[   .��5	      � .+�5
   
   �� -   5         ��Name$ IMG_Maze��5
      $
��Left=�6"   $	��348��6=   $��	10,10,0,0q�6J   $	��Top=�6]   $	��495��6u   $
��Fill=�6�   �� 5   
   -   5         ��Name$ label��5      $��Maze Solver��6   $��Center=�65   $��
510,10,0,0q�6R   $	��Top=�6f   ���	#FFD44807$
��Bold"�6�   $��18(�6�   $	��272��6�   $��Center=�6�   �� 5   
   -   5         ��Name$ LIST_Solution��5      $
��Left=�6)   $	��287��6D   $��
515,71,0,0q�6Q   $	��Top=�6e   $	��267��6}   �� 5   
   -   5         ��Name$ 
COMBO_Type��5       �� SelectedValuePath$ Content��6'   $
��Left=�6C   $��32��6^   $��260,394,0,0q�6j   $	��Top=�6   $	��245��6�   .@�5      �� .��6   Breadth First Search61   �� 5       �� 
IsSelected . .��6-   Depth First Search6A   �� 5      .��6   Iterative Depth First Search69   �� 5      .��6   Greedy Best First Search65   �� 5      .��6   	A* Search6&   5      �� 5   
   -   5        	 ��Name$	 button��5      $��Select an Image��6!   $
��Left=�6;   $��
10,394,0,0q�6V   $	��Top=�6j   $	��174��6�   $��32��6�   ���	#FFE05311
����������	#FFE053116�   �� 5   
   -   5        $ 
label_Copy��5      $��	Solve by:��6$   $��Center=�68   $��189,394,537,0q�6U   $	��Top=�6l   ���	#FFD44807$��66��6�   $��Right=�6�   $��32��6�   $��Center=�6�   �� 5   
   -   5        $ label_Copy1��5      $��Results:��6%   $��Center=�68   $��515,39,229,0q�6U   $	��Top=�6k   ���	#FFD44807$��48��6�   $��Right=�6�   $��32��6�   $��Center=�6�   �� 5   
   -   5        $	 	BTN_Solve��5      $��SOLVE��6$   $
��Left=�64   $��515,394,0,0q�6O   $	��Top=�6d   $	��267��6|   $��32��6�   ���	#FFE05311
����������	#FFE053116�   �� 5   
   -	   5        $	 BTN_Solve_Copy��5      $��Close��6)   $
��Left=�69   $��706,430,0,0q�6T   $	��Top=�6i   $��76��6�   $��32��6�   
����������	#FFE05311
�������6�   �� 5   
   -
   5        $	 	BTN_About��5      $��About��6$   $
��Left=�64   $��
10,430,0,0q�6O   $	��Top=�6c   $��76��6{   $��32��6�   
����������	#FFE05311
�������6�   �� 5   
   -   5        $ Lbl_coordinates��5      $��Elapsed Time:��6)   $��Center=�6A   $��11,363,386.6,0q�6^   $	��Top=�6v   ���	#FFD44807$	��396��6�   $��26��6�   $��Center=�6�   5      5      